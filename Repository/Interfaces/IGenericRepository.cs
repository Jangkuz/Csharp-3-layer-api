using Microsoft.EntityFrameworkCore.Query;
using Repository.Entities;
using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IGenericRepository<TEntity, TEntityId>
    where TEntity : BaseEntity<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    ///     <para>
    ///         Get entity from the DbSet of type <typeparamref name="TEntity"/> with primary key.
    ///     </para> 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the <typeparamref name="TEntity"/> entity with primary key matched with the input, else null if not found</returns>
    TEntity? GetById(TEntityId id);

    /// <summary>
    ///     <para>
    ///         Get entity from the DbSet of type <typeparamref name="TEntity"/> with primary key asynchronously.
    ///     </para> 
    /// </summary>
    /// <param name="id">The entity primary key value</param>
    /// <returns>The asynchronous task. The entity will be the result of the operation if found, else null</returns>
    Task<TEntity?> GetByIdAsync(TEntityId id);

    /// <summary>
    ///     <para>
    ///         Get an entity from the DbSet of type <typeparamref name="TEntity"/> with primary key <paramref name="id"/>.
    ///         Including all navigational properties from <paramref name="includeProperties"/>.
    ///     </para>
    /// </summary>
    /// <param name="id">The entity primary key</param>
    /// <param name="includeProperties">properties to be included in the query</param>
    /// <returns>The <typeparamref name="TEntity"/> entity if found, else null</returns>
    Task<TEntity?> GetByIdAsync(TEntityId id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>> includeProperties);

    /// <summary>
    ///     <para>
    ///         Get all existing entity from the DbSet of type <typeparamref name="TEntity"/>
    ///     </para>
    /// </summary>
    /// <returns>
    ///     A readonly <see cref="IReadOnlyCollection{T}"/> containing all existing records of type <typeparamref name="TEntity"/>
    /// </returns>
    Task<IReadOnlyCollection<TEntity>> ToListAsReadOnly();

    /// <summary>
    ///     Get an entity of type <typeparamref name="TEntity"/> with it primary key as a detached entity.
    ///     This entity won't be track by the DbContext untill being added again.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The entity of type <typeparamref name="TEntity"/> if found, else null</returns>
    TEntity? GetByIdAsDetached(TEntityId id);

    /// <summary>
    ///     <para>
    ///         Get all existing entities of type <typeparamref name="TEntity"/> with optional navigation properties in a 
    ///         <see cref="ICollection{T}"/>.
    ///     </para>
    /// </summary>
    /// <param name="includes">list of navigation properties for type of <typeparamref name="TEntity"/>.</param>
    /// <typeparam name="T">Type of entities the function should returned.</typeparam>
    /// <returns>
    ///     all existing records of type <typeparamref name="TEntity"/> with requested navigational properties included
    /// </returns>
    Task<ICollection<TEntity>> GetAllAsync(bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    ///     <para>
    ///         Get the first entity that match the <paramref name="filter"/> and return that entity with the 
    ///         included <paramref name="includeProperties"/> if found.
    ///     </para>
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includeProperties"></param>
    /// <returns>
    ///     The asynchronous task represent the search operation.
    ///     The result will be the <typeparamref name="TEntity"/> entity with included properties if found, else null.
    /// </returns>
    Task<TEntity?> GetByConditionAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProperties = null);

    /// <summary>
    ///     Return a list of entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <returns>
    ///     The asynchronous task represent the operation.
    ///     The result will be the enumeration list of existing entity of type <typeparamref name="TEntity"/>.
    /// </returns>
    Task<IEnumerable<TEntity>> ListAsync();

    /// <summary>
    ///     Return a list of entity of type <typeparamref name="TEntity"/> that match the <paramref name="filter"/>.
    ///     Optionally the operation will also sort the list based on the entity property.
    /// </summary>
    /// <param name="filter">The filter criteria for the entity searching progress</param>
    /// <param name="orderBy">The property which the list will be sorted by their value.</param>
    /// <returns>
    ///     The asynchronous task represent the operation.
    ///     The result will be the enumeration list of existing entity of type <typeparamref name="TEntity"/>.
    /// </returns>
    Task<IEnumerable<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool noTracking = true);

    /// <summary>
    ///     Return a list of entity of type <typeparamref name="TEntity"/> with <paramref name="includeProperties"/> included.
    ///     Optionally the operation will also filter the list by using the <paramref name="filter"/> 
    ///     and sort the list based on the entity <paramref name="orderBy"/> property.
    /// </summary>
    /// <param name="includeProperties">The included property for the entity fo type <typeparamref name="TEntity"/></param>
    /// <param name="filter">The filter criteria for the entity searching progress</param>
    /// <param name="orderBy">The property which the list will be sorted by their value.</param>
    /// <returns>
    ///     The asynchronous task represent the operation.
    ///     The result will be the enumeration list of existing entity of type <typeparamref name="TEntity"/>.
    /// </returns>
    Task<IEnumerable<TEntity>> ListAsync(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includeProperties,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool noTracking = true
    );

    /// <summary>
    ///     <para>
    ///         Find the first entity of type <typeparamref name="TEntity"/> that's match the given <see cref="Expression"/>.
    ///     </para>
    /// </summary>
    /// <param name="match">matching criteria function</param>
    /// <returns>The task result is the entity if found, else null</returns>
    TEntity? Find(Expression<Func<TEntity, bool>> match);

    /// <summary>
    ///     <para>
    ///         Asynchronously find the first entity of type <typeparamref name="TEntity"/> that's match the given 
    ///         <see cref="Expression"/>.
    ///     </para>
    /// </summary>
    /// <param name="match">matching criteria function</param>
    /// <returns>The task represent the asynchronous operation. The task result is the entity if found, else null</returns>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> match);

    /// <summary>
    ///     <para>
    ///         Asynchrounous method to start tracking a new entity of type <typeparamref name="TEntity"/>. 
    ///         This entity will be saved to the database when <see cref="DbContext.SaveChange()"/> is called.
    ///     </para>
    /// </summary>
    /// <param name="entity">The entity to be added</param>
    /// <returns>
    ///     The task that represent the asynchronous Add operation. 
    ///     The result of this operation is the entity that being tracked
    /// </returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    ///     <para>
    ///         Asynchronous operation to add a list of <typeparamref name="TEntity"/> entities and begin tracking them.
    ///     </para>
    /// </summary>
    /// <param name="entities">the list of entities to be added</param>
    /// <returns></returns>
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    ///     <para>
    ///         This operation is used to update an entity if the entity has a primary key, 
    ///         else add a new entity and begin tracking them.
    ///     </para>
    /// </summary>
    /// <param name="entity">The entity to be updated</param>
    TEntity Update(TEntity entity);

    /// <summary>
    ///     <para>
    ///         Change the state of the <paramref name="entity"/> to Deleted and will be removed from the database the next
    ///         time DbContext.SaveChange() is called.
    ///     </para>
    /// </summary>
    /// <param name="entity">The entity will be removed</param>
    void Delete(TEntity entity);

    /// <summary>
    ///     <para>
    ///         Change the state of the <paramref name="entity"/> to Deleted and will be removed from the database the next
    ///         time DbContext.SaveChange() is called.
    ///     </para>
    /// </summary>
    /// <param name="entity">The id of the entity will be removed</param>
    void Delete(TEntityId id);

    /// <summary>
    ///     Check if an entity is existed with it id.
    /// </summary>
    /// <param name="id">The id of the entity</param>
    /// <returns>true if the entity with that id is exist, else false</returns>
    bool Exists(TEntityId id);

    /// <summary>
    ///     Check if any existing entity of type <typeparamref name="TEntity"/> match the <paramref name="predicate"/>.
    /// </summary>
    /// <returns>true if at least one entity found that match the predicate, else false</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    ///     Get the number of entity currently exist.
    /// </summary>
    /// <returns>The number of entity in the context</returns>
    int Count();

    /// <summary>
    ///     The asynchronous version of <seealso cref="Count()"/> to get the number of entity currently exist.
    /// </summary>
    /// <returns>The number of entity in the context</returns>
    Task<int> CountAsync();
    Task DeleteRange(IEnumerable<TEntity> entities);

    //Task<PaginationResult<TEntity>> AsPaginated(int page, int pageSize, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? includes = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
}
