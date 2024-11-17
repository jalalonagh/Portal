using Microsoft.EntityFrameworkCore;
using JO.Response.Base;
using System.Linq.Expressions;

namespace JO.Data.Base.Interfaces
{
    public interface IEfCoreIdentityRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, IJOEntity
    {
        TDbContext GetDbContext();

        DbSet<TEntity> GetEntity();

        #region Insert
        Task<TEntity?> InsertAsync(CancellationToken cancellationToken, TEntity entity);

        Task<IEnumerable<TEntity>?> InsertManyAsync(CancellationToken cancellationToken,
            IEnumerable<TEntity> entities);
        #endregion

        #region Edit
        Task<TEntity?> UpdateAsync(CancellationToken cancellationToken, TEntity entity);

        Task<IEnumerable<TEntity>?> UpdateManyAsync(CancellationToken cancellationToken,
            IEnumerable<TEntity> entities);
        #endregion

        #region Delete
        Task<TEntity?> DeleteAsync(CancellationToken cancellationToken, TEntity entity);

        Task<IEnumerable<TEntity>?> DeleteManyAsync(CancellationToken cancellationToken,
            IEnumerable<TEntity> entities);
        #endregion

        #region Find
        Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, string>>[] includes);
        #endregion

        #region List
        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, params Expression<Func<TEntity, string>>[] includes);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, string>> order, bool asc);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate);

        Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken, PagedRequest pagedRequest);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, string>> order,
            bool asc);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, string>>[] includes);

        Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest pagedRequest,
            Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, string>> order,
            bool asc,
            params Expression<Func<TEntity, string>>[] includes);

        Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest pagedRequest,
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, string>>[] includes);
        #endregion
    }
}
