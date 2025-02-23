using Microsoft.EntityFrameworkCore.Query;
using SkinTime.DAL.Entities;
using System.Linq.Expressions;

namespace SkinTime.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        /// <summary>
        ///     <para>
        ///         Get entity from the DbSet of type <typeparamref name="T"/> with primary key.
        ///     </para> 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the <typeparamref name="T"/> entity with primary key matched with the input, else null if not found</returns>
        T? GetById(Guid id);

        /// <summary>
        ///     <para>
        ///         Get entity from the DbSet of type <typeparamref name="T"/> with primary key asynchronously.
        ///     </para> 
        /// </summary>
        /// <param name="id">The entity primary key value</param>
        /// <returns>The asynchronous task. The entity will be the result of the operation if found, else null</returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        ///     <para>
        ///         Get an entity from the DbSet of type <typeparamref name="T"/> with primary key <paramref name="id"/>.
        ///         Including all navigational properties from <paramref name="includeProperties"/>.
        ///     </para>
        /// </summary>
        /// <param name="id">The entity primary key</param>
        /// <param name="includeProperties">properties to be included in the query</param>
        /// <returns>The <typeparamref name="T"/> entity if found, else null</returns>
        Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object?>> includeProperties);

        /// <summary>
        ///     <para>
        ///         Get all existing entity from the DbSet of type <typeparamref name="T"/>
        ///     </para>
        /// </summary>
        /// <returns>
        ///     A readonly <see cref="IReadOnlyCollection{T}"/> containing all existing records of type <typeparamref name="T"/>
        /// </returns>
        Task<IReadOnlyCollection<T>> ToListAsReadOnly();

        /// <summary>
        ///     Get an entity of type <typeparamref name="T"/> with it primary key as a detached entity.
        ///     This entity won't be track by the DbContext untill being added again.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The entity of type <typeparamref name="T"/> if found, else null</returns>
        T? GetByIdAsDetached(Guid id);

        /// <summary>
        ///     <para>
        ///         Get all existing entities of type <typeparamref name="T"/> with optional navigation properties in a 
        ///         <see cref="ICollection{T}"/>.
        ///     </para>
        /// </summary>
        /// <param name="includes">list of navigation properties for type of <typeparamref name="T"/>.</param>
        /// <typeparam name="T">Type of entities the function should returned.</typeparam>
        /// <returns>
        ///     all existing records of type <typeparamref name="T"/> with requested navigational properties included
        /// </returns>
        Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

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
        ///     The result will be the <typeparamref name="T"/> entity with included properties if found, else null.
        /// </returns>
        Task<T?> GetByConditionAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null);

        /// <summary>
        ///     Return a list of entity of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        ///     The asynchronous task represent the operation.
        ///     The result will be the enumeration list of existing entity of type <typeparamref name="T"/>.
        /// </returns>
        Task<IEnumerable<T>> ListAsync();

        /// <summary>
        ///     Return a list of entity of type <typeparamref name="T"/> that match the <paramref name="filter"/>.
        ///     Optionally the operation will also sort the list based on the entity property.
        /// </summary>
        /// <param name="filter">The filter criteria for the entity searching progress</param>
        /// <param name="orderBy">The property which the list will be sorted by their value.</param>
        /// <returns>
        ///     The asynchronous task represent the operation.
        ///     The result will be the enumeration list of existing entity of type <typeparamref name="T"/>.
        /// </returns>
        Task<IEnumerable<T>> ListAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        /// <summary>
        ///     Return a list of entity of type <typeparamref name="T"/> with <paramref name="includeProperties"/> included.
        ///     Optionally the operation will also filter the list by using the <paramref name="filter"/> 
        ///     and sort the list based on the entity <paramref name="orderBy"/> property.
        /// </summary>
        /// <param name="includeProperties">The included property for the entity fo type <typeparamref name="T"/></param>
        /// <param name="filter">The filter criteria for the entity searching progress</param>
        /// <param name="orderBy">The property which the list will be sorted by their value.</param>
        /// <returns>
        ///     The asynchronous task represent the operation.
        ///     The result will be the enumeration list of existing entity of type <typeparamref name="T"/>.
        /// </returns>
        Task<IEnumerable<T>> ListAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object?>> includeProperties,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        );

        /// <summary>
        ///     <para>
        ///         Find the first entity of type <typeparamref name="T"/> that's match the given <see cref="Expression"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">matching criteria function</param>
        /// <returns>The task result is the entity if found, else null</returns>
        T? Find(Expression<Func<T, bool>> match);

        /// <summary>
        ///     <para>
        ///         Asynchronously find the first entity of type <typeparamref name="T"/> that's match the given 
        ///         <see cref="Expression"/>.
        ///     </para>
        /// </summary>
        /// <param name="match">matching criteria function</param>
        /// <returns>The task represent the asynchronous operation. The task result is the entity if found, else null</returns>
        Task<T?> FindAsync(Expression<Func<T, bool>> match);

        /// <summary>
        ///     <para>
        ///         Asynchrounous method to start tracking a new entity of type <typeparamref name="T"/>. 
        ///         This entity will be saved to the database when <see cref="DbContext.SaveChange()"/> is called.
        ///     </para>
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        /// <returns>
        ///     The task that represent the asynchronous Add operation. 
        ///     The result of this operation is the entity that being tracked
        /// </returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        ///     <para>
        ///         Asynchronous operation to add a list of <typeparamref name="T"/> entities and begin tracking them.
        ///     </para>
        /// </summary>
        /// <param name="entities">the list of entities to be added</param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        ///     <para>
        ///         This operation is used to update an entity if the entity has a primary key, 
        ///         else add a new entity and begin tracking them.
        ///     </para>
        /// </summary>
        /// <param name="entity">The entity to be updated</param>
        T Update(T entity);

        /// <summary>
        ///     <para>
        ///         Change the state of the <paramref name="entity"/> to Deleted and will be removed from the database the next
        ///         time DbContext.SaveChange() is called.
        ///     </para>
        /// </summary>
        /// <param name="entity">The entity will be removed</param>
        void Delete(T entity);

        /// <summary>
        ///     <para>
        ///         Change the state of the <paramref name="entity"/> to Deleted and will be removed from the database the next
        ///         time DbContext.SaveChange() is called.
        ///     </para>
        /// </summary>
        /// <param name="entity">The id of the entity will be removed</param>
        void Delete(Guid id);

        /// <summary>
        ///     Check if an entity is existed with it id.
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <returns>true if the entity with that id is exist, else false</returns>
        bool Exists(Guid id);

        /// <summary>
        ///     Check if any existing entity of type <typeparamref name="T"/> match the <paramref name="predicate"/>.
        /// </summary>
        /// <returns>true if at least one entity found that match the predicate, else false</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

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
    }
}
