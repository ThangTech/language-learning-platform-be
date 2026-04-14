using System.Linq.Expressions;

namespace Shared.Common.Repositories;

/// <summary>
/// Generic repository interface providing standard CRUD operations.
/// All specific repositories should extend this interface.
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Get entity by its primary key (Guid Id).
    /// </summary>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all entities.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Get entities with pagination, optional filtering and sorting.
    /// </summary>
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null);

    /// <summary>
    /// Find entities matching a predicate.
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Find a single entity matching a predicate.
    /// </summary>
    Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Add a new entity.
    /// </summary>
    Task AddAsync(T entity);

    /// <summary>
    /// Add multiple entities.
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Update an existing entity.
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Delete an entity.
    /// </summary>
    void Delete(T entity);

    /// <summary>
    /// Delete multiple entities.
    /// </summary>
    void DeleteRange(IEnumerable<T> entities);

    /// <summary>
    /// Check if any entity matches the predicate.
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Get the count of entities matching an optional predicate.
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

    /// <summary>
    /// Save all changes to the database.
    /// </summary>
    Task SaveChangesAsync();
}
