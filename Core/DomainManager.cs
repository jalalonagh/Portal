using Microsoft.EntityFrameworkCore;
using JO.Data.Base;
using JO.Data.Base.Interfaces;
using JO.Response.Base;
using System.Linq.Expressions;

namespace Core
{
    public class DomainManager<TEntity>
        where TEntity : class, IJOEntity
    {
        protected readonly IEfCoreRepository<TEntity> repository;

        public DomainManager(IEfCoreRepository<TEntity> _repository)
        {
            repository = _repository;
        }

        #region Delete
        public async Task<List<TEntity>?> DeleteDirectAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predict, bool autoSave = true)
        {
            return await repository.DeleteDirectAsync(cancellationToken, predict, autoSave).ConfigureAwait(false);
        }

        public async Task<TEntity?> DeleteAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
        {
            if (entity == null)
                throw new Exception("موجودیت بدون مقدار است");
            entity.Validate();

            return await repository.DeleteAsync(cancellationToken, entity, autoSave).ConfigureAwait(false);
        }

        public Task<IEnumerable<TEntity>?> DeleteManyAsync(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = true)
        {
            foreach (var item in entities)
                if (item == null)
                    throw new Exception("موجودیت بدون مقدار است");
                else
                    item.Validate();

            return repository.DeleteManyAsync(cancellationToken, entities, autoSave);
        }
        #endregion

        #region Find
        public async Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        {
            var result = await repository.FindAsync(cancellationToken, predicate).ConfigureAwait(false);

            return result;
        }

        public async Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            return await repository.FindAsync(cancellationToken, predicate, includes).ConfigureAwait(false);
        }
        #endregion

        #region List
        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken)
        {
            return await repository.GetListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, params string[] includes)
        {
            return await repository.GetListAsync(cancellationToken, includes).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, string order, bool asc)
        {
            return await repository.GetListAsync(cancellationToken, order, asc).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        {
            return await repository.GetListAsync(cancellationToken, predicate).ConfigureAwait(false);
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken, PagedRequest<TEntity> pagedRequest)
        {
            return await repository.GetListAsync(cancellationToken, pagedRequest).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string order, bool asc)
        {
            return await repository.GetListAsync(cancellationToken, predicate, order, asc).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            return await repository.GetListAsync(cancellationToken, predicate, includes).ConfigureAwait(false);
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken, PagedRequest<TEntity> pagedRequest, Expression<Func<TEntity, bool>> predicate)
        {
            return await repository.GetListAsync(cancellationToken, pagedRequest, predicate).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate, string order, bool asc, params string[] includes)
        {
            return await repository.GetListAsync(cancellationToken, predicate, order, asc, includes).ConfigureAwait(false);
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken, PagedRequest<TEntity> pagedRequest, Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            return await repository.GetListAsync(cancellationToken, pagedRequest, predicate, includes).ConfigureAwait(false);
        }

        public async Task<PagedResultV2<TEntity>?> GetListV2Async(CancellationToken cancellationToken, PagedRequestV2 pagedRequest, Expression<Func<TEntity, bool>>? predicate = null)
        {
            return await repository.GetListV2Async(cancellationToken, pagedRequest, predicate);
        }
        #endregion

        #region Insert
        public async Task<TEntity?> InsertAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
        {
            if (entity == null)
                throw new Exception("موجودیت بدون مقدار است");

            entity.Validate();

            return await repository.InsertAsync(cancellationToken, entity, autoSave).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> InsertManyAsync(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = true)
        {
            foreach (var item in entities)
                if (item == null)
                    throw new Exception("موجودیت بدون مقدار است");
                else
                    item.Validate();

            return await repository.InsertManyAsync(cancellationToken, entities, autoSave).ConfigureAwait(false);
        }
        #endregion

        #region Update
        public async Task<TEntity?> UpdateAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
        {
            if (entity == null)
                throw new Exception("موجودیت بدون مقدار است");

            entity.Validate();

            return await repository.UpdateAsync(cancellationToken, entity, autoSave).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> UpdateManyAsync(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = true)
        {
            foreach (var item in entities)
                if (item == null)
                    throw new Exception("موجودیت بدون مقدار است");
                else
                    item.Validate();

            return await repository.UpdateManyAsync(cancellationToken, entities, autoSave).ConfigureAwait(false);
        }
        #endregion
    }
}
