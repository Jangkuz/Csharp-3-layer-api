using Repository.Entities;
using Repository.Interfaces;

namespace Repository.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    //IManufactureRepository ManufactureRepository { get; }

    IGenericRepository<TEntity, TEntityId> GetRepo<TEntity, TEntityId>()
        where TEntity : BaseEntity<TEntityId>
        where TEntityId : notnull;
    Task SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollBackAsync();
    Task<bool> SaveAsync();
}
