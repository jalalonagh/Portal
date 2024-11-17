using Microsoft.EntityFrameworkCore;
using JO.Response.Base;
using System.Linq.Expressions;

namespace JO.Data.Base.Interfaces
{
    public interface IEfCoreRepository<TEntity>
        where TEntity : class, IJOEntity
    {
        DbSet<TEntity> GetDb();
        DbSet<T> GetDb<T>() where T : class, IJOEntity;

        #region Insert
        Task<TEntity?> InsertAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true);

        Task<IEnumerable<TEntity>?> InsertManyAsync(CancellationToken cancellationToken,
            IEnumerable<TEntity> entities,
            bool autoSave = true);
        #endregion

        #region Edit
        Task<TEntity?> UpdateAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true);

        Task<IEnumerable<TEntity>?> UpdateManyAsync(CancellationToken cancellationToken,
            IEnumerable<TEntity> entities,
            bool autoSave = true);
        #endregion

        #region Delete
        Task<TEntity?> DeleteAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true);

        Task<IEnumerable<TEntity>?> DeleteManyAsync(CancellationToken cancellationToken,
            IEnumerable<TEntity> entities,
            bool autoSave = true);

        Task<List<TEntity>?> DeleteDirectAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predict, bool autoSave = true);
        #endregion

        #region Find
        Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate,
            params string[] includes);
        #endregion

        #region List
        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, params string[] includes);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, string order, bool asc);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate);

        Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken, PagedRequest<TEntity> pagedRequest);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            string order,
            bool asc);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            params string[] includes);

        Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest<TEntity> pagedRequest,
            Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            string order,
            bool asc,
            params string[] includes);

        Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest<TEntity> pagedRequest,
            Expression<Func<TEntity, bool>> predicate,
            params string[] includes);

        Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest<TEntity> pagedRequest,
            params string[] includes);
        Task<PagedResultV2<TEntity>?> GetListV2Async(CancellationToken cancellationToken, PagedRequestV2 pagedRequest, Expression<Func<TEntity, bool>>? predicate = null);

        #endregion
    }
}
