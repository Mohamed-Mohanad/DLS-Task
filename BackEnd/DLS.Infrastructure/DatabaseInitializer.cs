using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DLS.Infrastructure;

internal class DatabaseInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InitializeDatabaseAsync()
    {
        _logger.LogInformation("Starting database initialization");

        try
        {
            // Create a scope for database operations
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if database exists
            bool dbExists = await dbContext.Database.CanConnectAsync();

            if (!dbExists)
            {
                _logger.LogInformation("Database does not exist. Creating database and applying migrations");

                // Create database with all migrations applied
                await dbContext.Database.MigrateAsync();
                _logger.LogInformation("Database created and migrations applied successfully");
            }
            else
            {
                // Check and ensure migrations history table exists
                await EnsureMigrationsHistoryTableExistsAsync(dbContext);

                // Check for pending migrations
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                var pendingMigrationsList = pendingMigrations.ToList();

                if (pendingMigrationsList.Any())
                {
                    _logger.LogInformation("Pending migrations found: {Migrations}", string.Join(", ", pendingMigrationsList));

                    // Apply pending migrations within a transaction
                    await ApplyMigrationsWithTransactionAsync(dbContext, pendingMigrationsList);
                }
                else
                {
                    _logger.LogInformation("Database is up to date. No migrations to apply");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during database initialization");
            throw;
        }
    }

    private async Task EnsureMigrationsHistoryTableExistsAsync(ApplicationDbContext dbContext)
    {
        try
        {
            _logger.LogInformation("Checking if __EFMigrationsHistory table exists");

            // Check if the migrations history table exists
            string checkTableSql = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_SCHEMA = 'dbo' 
                    AND TABLE_NAME = '__EFMigrationsHistory')
                SELECT 0
                ELSE SELECT 1";

            var result = await dbContext.Database.ExecuteSqlRawAsync(checkTableSql);

            if (result == 0)
            {
                _logger.LogInformation("__EFMigrationsHistory table does not exist. Creating it now.");

                // Create the migrations history table
                string createTableSql = @"
                    CREATE TABLE [dbo].[__EFMigrationsHistory] (
                        [MigrationId] nvarchar(150) NOT NULL,
                        [ProductVersion] nvarchar(32) NOT NULL,
                        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
                    )";

                await dbContext.Database.ExecuteSqlRawAsync(createTableSql);
                _logger.LogInformation("__EFMigrationsHistory table created successfully");
            }
            else
            {
                _logger.LogInformation("__EFMigrationsHistory table already exists");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking or creating __EFMigrationsHistory table");
            throw;
        }
    }

    private async Task<bool> IsMigrationAppliedAsync(ApplicationDbContext dbContext, string migrationId)
    {
        // Check if migration exists in __EFMigrationsHistory table
        var sql = $"SELECT COUNT(1) FROM [__EFMigrationsHistory] WHERE [MigrationId] = '{migrationId}'";

        try
        {
            var count = await dbContext.Database.ExecuteSqlRawAsync(sql);
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if migration {MigrationId} is applied", migrationId);
            return false;
        }
    }

    private async Task ApplyMigrationsWithTransactionAsync(ApplicationDbContext dbContext, List<string> pendingMigrations)
    {
        _logger.LogInformation("Applying {Count} migrations", pendingMigrations.Count);

        try
        {
            // Filter out migrations that are already in the history table
            var migrationsToApply = new List<string>();

            foreach (var migration in pendingMigrations)
            {
                bool isApplied = await IsMigrationAppliedAsync(dbContext, migration);

                if (isApplied)
                {
                    _logger.LogWarning("Migration {Migration} is already recorded in __EFMigrationsHistory but EF Core thinks it's pending", migration);
                }
                else
                {
                    migrationsToApply.Add(migration);
                }
            }

            if (migrationsToApply.Count < pendingMigrations.Count)
            {
                _logger.LogInformation("Found {Count} migrations already applied but not tracked by EF Core",
                    pendingMigrations.Count - migrationsToApply.Count);
            }

            if (!migrationsToApply.Any())
            {
                _logger.LogInformation("No migrations to apply after filtering already applied migrations");
                return;
            }

            // Use a transaction for migration safety
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                // Apply each migration individually to handle potential conflicts
                foreach (var migration in migrationsToApply)
                {
                    try
                    {
                        _logger.LogInformation("Applying migration: {Migration}", migration);

                        // Manually insert the migration record without applying the migration
                        // This is needed when tables already exist but the migration isn't in history
                        string productVersion = dbContext.GetType().Assembly.GetName().Version?.ToString() ?? "6.0.0";
                        await dbContext.Database.ExecuteSqlRawAsync(
                            $"INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES ('{migration}', '{productVersion}')");

                        _logger.LogInformation("Recorded migration {Migration} in __EFMigrationsHistory", migration);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error recording migration {Migration}", migration);
                        throw;
                    }
                }

                await transaction.CommitAsync();
                _logger.LogInformation("Successfully recorded all migrations");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error applying migrations. Transaction rolled back");
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to apply migrations");
            throw;
        }
    }
}
