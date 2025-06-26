using DataAccessLayer.Interfaces;
using Repository.Entities;

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
