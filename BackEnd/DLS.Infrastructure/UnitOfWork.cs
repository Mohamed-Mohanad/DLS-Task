using DLS.Infrastructure.Repositories;
using Domain.Repositories;
using System.Collections;

namespace DLS.Infrastructure;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private Hashtable _repositories = new();

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories is null)
            _repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repository = new GenericRepository<TEntity>(_context);
            _repositories.Add(type, repository);
        }

        return (IGenericRepository<TEntity>)_repositories[type]!;
    }
}
