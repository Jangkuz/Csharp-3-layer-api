using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Paging;
using System.Linq.Expressions;

namespace Repository.Implements;

public class GenericRepository<TEntity, TEntityId> : IGenericRepository<TEntity, TEntityId>
    where TEntity : BaseEntity<TEntityId>
    where TEntityId : notnull
{
    protected readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<TEntity> AddAsync(TEntity entity)
    {
        EntityEntry<TEntity> entityTracker = await _context.Set<TEntity>().AddAsync(entity);
        return entityTracker.Entity;
    }

    public int Count()
    {
        return _context.Set<TEntity>().Count();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<TEntity>().CountAsync();
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void Delete(TEntityId id)
    {
        var entity = _context.Set<TEntity>().Find(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }

    public bool Exists(TEntityId id)
    {
        return _context.Set<TEntity>().Any(e => e.Id.Equals(id));
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().AnyAsync(predicate);
    }

    public TEntity? Find(Expression<Func<TEntity, bool>> match)
    {
        return _context.Set<TEntity>().SingleOrDefault(match);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> match)
    {
        return await _context.Set<TEntity>().SingleOrDefaultAsync(match);
    }

    public async Task<TEntity?> FindAsync(
    Expression<Func<TEntity, bool>> filter,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProperties = null)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (includeProperties != null)
        {
            query = includeProperties(query);
        }

        return await query.FirstOrDefaultAsync(filter);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>();
    }

    public TEntity? GetById(TEntityId id)
    {
        return _context.Set<TEntity>().Find(id);
    }

    public TEntity? GetByIdAsDetached(TEntityId id)
    {
        var entity = _context.Set<TEntity>().Find(id);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
        return entity;
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>> includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (includeProperties != null)
        {
            query = includeProperties(query);
        }

        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<IReadOnlyCollection<TEntity>> ToListAsReadOnly()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
        if (noTracking)
        {
            query.AsNoTracking();
        }
        return await query.ToListAsync();
    }

    public async Task<ICollection<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includeProperties,
        bool noTracking = true
        )
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        if (includeProperties != null)
        {
            query = includeProperties(query);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        bool noTracking = true
    )
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includeProperties,
        Expression<Func<TEntity, bool>>? filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        bool noTracking = true
    )
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            query = includeProperties(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includeProperties, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool noTracking = true)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            query = includeProperties(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        else
        {
            query = query.OrderBy(t => t.Id);
        }

        if (pageNumber >= 1 && pageSize >= 1)
        {
            query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        }

        return await query.ToListAsync();
    }

    public TEntity Update(TEntity entity)
    {
        EntityEntry<TEntity> entityTracker = _context.Set<TEntity>().Update(entity);
        return entityTracker.Entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _context.AddRangeAsync(entities);
    }

    public Task DeleteRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task<PaginationResult<TEntity>> AsPaginatedInRAM(
    int page,
    int pageSize,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null,
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        IEnumerable<TEntity> items = await GetAllAsync(includeProperties: includes, filter: filter, orderBy: orderBy);

        Console.WriteLine(items.Count());


        return new PaginationResult<TEntity>
        {
            Content = items.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
            ItemAmount = items.Count(),
            CurrentPage = page,
            PageSize = pageSize,
        };
    }

    public async Task<PaginationResult<TEntity>> AsPaginated(
        int page,
        int pageSize,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        IEnumerable<TEntity> items = await GetAllAsync(
            pageNumber: page,
            pageSize: pageSize,
            includeProperties: includes,
            filter: filter,
            orderBy: orderBy);

        Console.WriteLine(items.Count());

        return new PaginationResult<TEntity>
        {
            Content = items.ToList(),
            ItemAmount = items.Count(),
            CurrentPage = page,
            PageSize = pageSize,
        };
    }

}
