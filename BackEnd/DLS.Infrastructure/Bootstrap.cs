using DLS.Infrastructure.Options;
using DLS.Infrastructure.Repositories;
using Domain.Repositories;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DLS.Infrastructure;

public static class Bootstrap
{
    public static async Task<IServiceCollection> AddInfrastructureStrapping(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddAuth(configuration);

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbConfig(configuration);

        services.AddSingleton<DatabaseInitializer>();

        var serviceProvider = services.BuildServiceProvider();

        await serviceProvider.GetRequiredService<DatabaseInitializer>().InitializeDatabaseAsync();

        return services;
    }

    private static IServiceCollection AddDbConfig(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database")!;

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("A valid database connection string must be provided.");

        services.AddSingleton<AuditableEntitiesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(connectionString);
                optionsBuilder.AddInterceptors(sp.GetRequiredService<AuditableEntitiesInterceptor>());
            });

        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);

        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        return services;
    }
}
