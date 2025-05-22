using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction _transaction = null!;
    private IServiceProvider _serviceProvider;


    //public IManufactureRepository ManufactureRepository { get; private set; }

    public UnitOfWork(
        AppDbContext context,
        IServiceProvider serviceProvider
        //IManufactureRepository manufacture
        )
    {
        _context = context;
        _serviceProvider = serviceProvider;

        //ManufactureRepository = manufacture;
    }

    public IGenericRepository<TEntity, TEntityId> GetRepo<TEntity, TEntityId>()
        where TEntity : BaseEntity<TEntityId>
        where TEntityId : notnull
    {
        return _serviceProvider.GetRequiredService<IGenericRepository<TEntity, TEntityId>>();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    private bool disposed = false;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }
    }


    public async Task RollBackAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null!;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
