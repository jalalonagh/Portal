using Microsoft.EntityFrameworkCore;
using JO.Data.Base;
using JO.Data.Base.Interfaces;
using JO.Response.Base;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.DB
{
    public class EfCoreIdentityRepository<TDbContext, TEntity> : IEfCoreIdentityRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, IJOEntity
    {
        private readonly TDbContext context;
        private readonly DbSet<TEntity> dbset;
        private readonly IUnitOfWork<TDbContext> unitOfWork;

        public EfCoreIdentityRepository(TDbContext _context, IUnitOfWork<TDbContext> _unitOfWork)
        {
            this.context = _context;
            dbset = context.Set<TEntity>();
            this.unitOfWork = _unitOfWork;
        }

        public TDbContext GetDbContext()
        {
            return context;
        }

        public DbSet<TEntity> GetEntity()
        {
            return dbset;
        }

        #region Insert
        public async Task<TEntity?> InsertAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
        {
            entity.ReNewConcurrency();

            entity.SetTimeNow();

            var savedEntity = (await dbset.AddAsync(entity, cancellationToken).ConfigureAwait(false)).Entity;

            if (autoSave)
            {
                var result = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result >= 1 ? savedEntity : null;
            }

            return savedEntity;
        }

        public async Task<IEnumerable<TEntity>?> InsertManyAsync(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = true)
        {
            if (entities == null || !entities.Any())
                return null;

            foreach (var item in entities)
            {
                item.ReNewConcurrency();
                item.SetTimeNow();
            }

            await dbset.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

            if (autoSave)
            {
                var result = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result >= entities.Count() ? entities : null;
            }

            return entities;
        }
        #endregion

        #region Edit
        public async Task<TEntity?> UpdateAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
        {
            var ccy = (await dbset.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(entity.Id)).ConfigureAwait(false))?.ConcurrencyStatus;

            if (ccy == null || entity.ConcurrencyStatus != ccy)
                throw new Exception("مقدار Concurency صحیح نمی باشد.");

            entity.ReNewConcurrency();
            entity.SetTimeNow();

            if (dbset.Local.All(e => e != entity))
            {
                dbset.Attach(entity);
                context.Update(entity);
            }

            if (autoSave)
            {
                var result = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result >= 1 ? entity : null;
            }

            return entity;
        }

        public async Task<IEnumerable<TEntity>?> UpdateManyAsync(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = true)
        {
            if (entities == null || !entities.Any())
                return null;

            var items = await dbset.AsNoTracking()
                .Where(w => entities.Select(s => s.Id).Contains(w.Id))
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            foreach (var item in entities)
            {
                var ccy = (items.FirstOrDefault(f => f.Id.Equals(item.Id)))?.ConcurrencyStatus;

                if (ccy == null || item.ConcurrencyStatus != ccy)
                    throw new Exception("یکی از مقادیر Concurency صحیح ندارد.");

                item.SetTimeNow();
                item.ReNewConcurrency();
            }

            context.UpdateRange(entities);

            if (autoSave)
            {
                var result = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result >= entities.Count() ? entities : null;
            }

            return entities;
        }
        #endregion

        #region Delete
        public async Task<TEntity?> DeleteAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
        {
            var ccy = (await dbset.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(entity.Id)).ConfigureAwait(false))?.ConcurrencyStatus;

            if (ccy == null || entity.ConcurrencyStatus != ccy)
                throw new Exception("مقدار Concurency صحیح نمی باشد.");

            entity.SetTimeNow();

            dbset.Remove(entity);

            if (autoSave)
            {
                var result = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result >= 1 ? entity : null;
            }

            return entity;
        }

        public async Task<IEnumerable<TEntity>?> DeleteManyAsync(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = false)
        {
            if (entities == null || !entities.Any())
                return null;

            var items = await dbset.AsNoTracking()
                .Where(w => entities.Select(s => s.Id).Contains(w.Id))
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            foreach (var item in entities)
            {
                var ccy = (items.FirstOrDefault(f => f.Id.Equals(item.Id)))?.ConcurrencyStatus;

                if (ccy == null || item.ConcurrencyStatus != ccy)
                    throw new Exception("یکی از مقادیر Concurency صحیح ندارد.");

                item.SetTimeNow();
            }

            context.RemoveRange(entities.Select(x => x));

            if (autoSave)
            {
                var result = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result >= entities.Count() ? entities : null;
            }

            return entities;
        }
        #endregion

        #region Find
        public async Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        {
            return await dbset
                .AsNoTracking()
                .Where(predicate)
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, string>>[] includes)
        {
            var query = dbset
                .AsNoTracking()
                .Where(predicate);

            if (includes != null && includes.Any())
                foreach (var item in includes)
                    query = query.Include(item);

            return await query
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region List
        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken)
        {
            var query = dbset
                .AsNoTracking()
                .OrderByDescending(o => o.Id)
                .AsQueryable();

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, params Expression<Func<TEntity, string>>[] includes)
        {
            var query = dbset
                .AsNoTracking()
                .OrderByDescending(o => o.Id)
                .AsQueryable();

            if (includes != null && includes.Any())
                foreach (var item in includes)
                    query = query.Include(item);

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, string>> order, bool asc)
        {
            var query = dbset
                .AsNoTracking()
                .AsQueryable();

            if (order != null)
            {
                query = ApplyOrderDirection<TEntity>(query, order.Name, asc);
            }

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
        {
            var query = dbset
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(o => o.Id)
                .AsQueryable();

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken, PagedRequest<TEntity> pagedRequest)
        {
            var queryable = dbset
                .AsNoTracking();

            var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);

            if (pagedRequest.Sorting != null)
            {
                queryable = ApplyOrderDirection<TEntity>(queryable, pagedRequest.Sorting, pagedRequest.IsAsc ?? false);
            }

            var items = await queryable
                .Skip(pagedRequest.SkipCount ?? 0)
                .Take(pagedRequest.MaxResultCount ?? 0)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedResult<TEntity>(count, items);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, string>> order,
            bool asc)
        {
            var query = dbset
                .AsNoTracking()
                .Where(predicate)
                .AsQueryable();

            if (order != null)
            {
                query = ApplyOrderDirection<TEntity>(query, order.Name, asc);
            }

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, string>>[] includes)
        {
            var query = dbset
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(o => o.Id)
                .AsQueryable();

            if (includes != null && includes.Any())
                foreach (var item in includes)
                    query = query.Include(item);

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest<TEntity> pagedRequest,
            Expression<Func<TEntity, bool>> predicate)
        {
            var queryable = dbset
                .AsNoTracking()
                .Where(predicate);

            var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);

            if (pagedRequest.Sorting != null)
            {
                queryable = ApplyOrderDirection<TEntity>(queryable, pagedRequest.Sorting, pagedRequest.IsAsc ?? false);
            }

            var items = await queryable
                .Skip(pagedRequest.SkipCount ?? 0)
                .Take(pagedRequest.MaxResultCount ?? 0)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedResult<TEntity>(count, items);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, string>> order,
            bool asc,
            params Expression<Func<TEntity, string>>[] includes)
        {
            var query = dbset
                .AsNoTracking()
                .Where(predicate)
                .AsQueryable();

            if (order != null)
            {
                query = ApplyOrderDirection<TEntity>(query, order.Name, asc);
            }

            if (includes != null && includes.Any())
                foreach (var item in includes)
                    query = query.Include(item);

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest<TEntity> pagedRequest,
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, string>>[] includes)
        {
            var queryable = dbset
                .AsNoTracking()
                .Where(predicate);

            var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);

            if (pagedRequest.Sorting != null)
            {
                queryable = ApplyOrderDirection<TEntity>(queryable, pagedRequest.Sorting, pagedRequest.IsAsc ?? false);
            }

            if (includes != null && includes.Any())
                foreach (var item in includes)
                    queryable = queryable.Include(item);

            var items = await queryable
                .Skip(pagedRequest.SkipCount ?? 0)
                .Take(pagedRequest.MaxResultCount ?? 0)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedResult<TEntity>(count, items);
        }
        #endregion

        protected async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private IOrderedQueryable<TSource> ApplyOrderDirection<TSource>(IQueryable<TSource> source, string attributeName, bool isAsc)
        {
            if (String.IsNullOrEmpty(attributeName))
            {
                return source as IOrderedQueryable<TSource>;
            }

            var propertyInfo = typeof(TSource).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new ArgumentException("ستون مورد نظر برای مرتب سازی وجود ندارد", attributeName);
            }

            Expression<Func<TSource, object>> orderExpression = x => propertyInfo.GetValue(x, null);

            if (isAsc)
            {
                return source.OrderBy(orderExpression);
            }
            else
            {
                return source.OrderByDescending(orderExpression);
            }
        }
    }
}
