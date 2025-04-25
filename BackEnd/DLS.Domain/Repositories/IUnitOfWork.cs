namespace Domain.Repositories;
public interface IUnitOfWork
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}