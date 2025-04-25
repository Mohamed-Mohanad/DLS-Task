using Domain.Repositories;
using Domain.Specification;
using Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DLS.Infrastructure.Repositories;

internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _entity;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _entity = context.Set<TEntity>();
    }

    public bool HasData()
        => _entity.Any();

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _entity.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        const int batchSize = 1000;
        for (var i = 0; i < entities.Count; i += batchSize)
        {
            var batch = entities.Skip(i).Take(batchSize).ToList();
            await _entity.AddRangeAsync(batch, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public void Delete(TEntity entity)
        => _entity.Remove(entity);

    public void DeleteRange(IEnumerable<TEntity> entities)
        => _entity.RemoveRange(entities);

    public void Update(TEntity entity)
        => _entity.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities)
        => _entity.UpdateRange(entities);

    public async Task<TEntity?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        => await _entity.FindAsync(id, cancellationToken);

    public async Task<(IQueryable<TEntity> data, int count)> GetWithSpecAsync(
        Specification<TEntity> specifications,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator<TEntity>.GetQueryAsync(_entity, specifications, cancellationToken);
    }

    public async Task<TEntity?> GetEntityWithSpecAsync(
        Specification<TEntity> specifications,
        CancellationToken cancellationToken = default)
    {
        var (query, _) = await GetWithSpecAsync(specifications, cancellationToken);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        => await _entity.AnyAsync(filter, cancellationToken);

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken) > 0;

    public IReadOnlyList<TEntity> GetAll()
        => _entity.AsNoTracking().ToList();

    public void ExecuteUpdateRange(Expression<Func<TEntity, bool>> filter,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> expression)
    {
        _entity.Where(filter)
            .ExecuteUpdate(expression);
    }
}
