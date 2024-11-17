using Infrastructure.DB.Context.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using JO.Data.Base;
using JO.Data.Base.Interfaces;
using JO.Response.Base;
using JO.Shared.Interfaces;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.DB
{
    public class EfCoreRepository<TEntity> : IEfCoreRepository<TEntity>
        where TEntity : class, IJOEntity
    {
        private readonly MembershipEFCoreContext context;
        private readonly DbSet<TEntity> dbset;
        private readonly IUnitOfWork<MembershipEFCoreContext> unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IConfiguration _configuration;

        public EfCoreRepository(MembershipEFCoreContext _context,
            IUnitOfWork<MembershipEFCoreContext> _unitOfWork,
            ICurrentUserService currentUser,
            IConfiguration configuration)
        {
            this.context = _context;
            this.unitOfWork = _unitOfWork;
            dbset = unitOfWork.GetDatabase().Set<TEntity>();
            _currentUser = currentUser;
            _configuration = configuration;
        }

        public DbSet<TEntity> GetDb()
        {
            return dbset;
        }

        public DbSet<T> GetDb<T>()
            where T : class, IJOEntity
        {
            return context.Set<T>();
        }

        #region Insert
        public async Task<TEntity?> InsertAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
        {
            if (entity == null)
            {
                return null;
            }

            entity.ReNewConcurrency();
            entity.SetTimeNow();

            if (entity.GetIsSystemAction())
            {
                var sys = _configuration.GetSection("SystemUser").Get<long>();

                entity.SetEditor(sys);
            }
            else
            {
                entity.SetEditor(_currentUser.UserId);
            }

            var savedEntity = (await unitOfWork._context.AddAsync(entity, cancellationToken).ConfigureAwait(false)).Entity;

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
            {
                return null;
            }

            foreach (var item in entities)
            {
                if (item == null)
                {
                    return null;
                }

                item.ReNewConcurrency();
                item.SetTimeNow();

                if (item.GetIsSystemAction())
                {
                    var sys = _configuration.GetSection("SystemUser").Get<long>();

                    item.SetEditor(sys);
                }
                else
                {
                    item.SetEditor(_currentUser.UserId);
                }
            }

            await unitOfWork._context.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

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
            if (entity == null)
            {
                return null;
            }

            var ccy = (await dbset.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(entity.Id)).ConfigureAwait(false))?.ConcurrencyStatus;

            if (ccy == null || entity.ConcurrencyStatus != ccy)
            {
                return null;
            }

            entity.ReNewConcurrency();
            entity.SetTimeNow();

            if (entity.GetIsSystemAction())
            {
                var sys = _configuration.GetSection("SystemUser").Get<long>();

                entity.SetEditor(sys);
            }
            else
            {
                entity.SetEditor(_currentUser.UserId);
            }

            if (dbset.Local.All(e => e != entity))
            {
                unitOfWork._context.Attach(entity);
                unitOfWork._context.Update(entity);
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
            {
                return null;
            }

            var items = await dbset.AsNoTracking()
                .Where(w => entities.Select(s => s.Id).Contains(w.Id))
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            foreach (var item in entities)
            {
                if (item == null)
                {
                    return null;
                }

                var ccy = (items.FirstOrDefault(f => f.Id.Equals(item.Id)))?.ConcurrencyStatus;

                if (ccy == null || item.ConcurrencyStatus != ccy)
                {
                    return null;
                }

                item.ReNewConcurrency();
                item.SetTimeNow();

                if (item.GetIsSystemAction())
                {
                    var sys = _configuration.GetSection("SystemUser").Get<long>();

                    item.SetEditor(sys);
                }
                else
                {
                    item.SetEditor(_currentUser.UserId);
                }
            }

            unitOfWork._context.UpdateRange(entities);

            if (autoSave)
            {
                var result = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result >= entities.Count() ? entities : null;
            }

            return entities;
        }
        #endregion

        #region Delete
        public async Task<List<TEntity>?> DeleteDirectAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predict, bool autoSave = true)
        {
            var found = await dbset.Where(predict).ToListAsync(cancellationToken).ConfigureAwait(false);

            if (found == null || !found.Any())
            {
                return null;
            }

            foreach (var entity in found)
            {
                entity.SetTimeNow();
                entity.SetDeleted();
                if (entity.GetIsSystemAction())
                {
                    var sys = _configuration.GetSection("SystemUser").Get<long>();

                    entity.SetEditor(sys);
                }
                else
                    entity.SetEditor(_currentUser.UserId);
            }

            unitOfWork._context.UpdateRange(found);

            if (autoSave)
            {
                var result = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return result >= 1 ? found : null;
            }

            return found;
        }

        public async Task<TEntity?> DeleteAsync(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
        {
            var ccy = (await dbset.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(entity.Id)).ConfigureAwait(false))?.ConcurrencyStatus;

            if (ccy == null || entity.ConcurrencyStatus != ccy)
            {
                throw new Exception("مقدار Concurency صحیح نمی باشد.");
            }

            entity.SetTimeNow();
            entity.SetDeleted();

            if (entity.GetIsSystemAction())
            {
                var sys = _configuration.GetSection("SystemUser").Get<long>();

                entity.SetEditor(sys);
            }
            else
            {
                entity.SetEditor(_currentUser.UserId);
            }

            unitOfWork._context.Update(entity);

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
            {
                return null;
            }

            var items = await dbset.AsNoTracking()
                .Where(w => entities.Select(s => s.Id).Contains(w.Id))
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            foreach (var item in entities)
            {
                var ccy = (items.FirstOrDefault(f => f.Id.Equals(item.Id)))?.ConcurrencyStatus;

                if (ccy == null || item.ConcurrencyStatus != ccy)
                {
                    throw new Exception("یکی از مقادیر Concurency صحیح ندارد.");
                }

                item.SetTimeNow();
                item.SetDeleted();

                if (item.GetIsSystemAction())
                {
                    var sys = _configuration.GetSection("SystemUser").Get<long>();

                    item.SetEditor(sys);
                }
                else
                {
                    item.SetEditor(_currentUser.UserId);
                }
            }

            unitOfWork._context.UpdateRange(entities.Select(x => x));

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
            var result = await dbset
                .AsNoTracking()
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            return result;
        }

        public async Task<TEntity?> FindAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate,
            params string[] includes)
        {
            var query = dbset
                .AsNoTracking()
                .Where(predicate);

            if (includes != null && includes.Any())
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            return await query
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
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

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, params string[] includes)
        {
            var query = dbset
                .AsNoTracking()
                .OrderByDescending(o => o.Id)
                .AsQueryable();

            if (includes != null && includes.Any())
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken, string order, bool asc)
        {
            var items = await dbset
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (order != null)
            {
                return (ApplyOrderDirection<TEntity>(items.AsQueryable(), order, asc)).ToList();
            }

            return items;
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

            var items = await queryable
                .Skip(pagedRequest.SkipCount ?? 0)
                .Take(pagedRequest.MaxResultCount ?? 0)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            if (pagedRequest.Sorting != null)
            {
                items = (ApplyOrderDirection<TEntity>(items.AsQueryable(), pagedRequest.Sorting, pagedRequest.IsAsc ?? false)).ToList();
            }

            return new PagedResult<TEntity>(count, items);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            string order,
            bool asc)
        {
            var items = await dbset
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (order != null)
            {
                return (ApplyOrderDirection<TEntity>(items.AsQueryable(), order, asc)).ToList();
            }

            return items;
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            params string[] includes)
        {
            var query = dbset
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(o => o.Id)
                .AsQueryable();

            if (includes != null && includes.Any())
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest<TEntity> pagedRequest,
            Expression<Func<TEntity, bool>> predicate)
        {
            var queryable = dbset
                .AsNoTracking()
                .Where(predicate)
                .AsQueryable();

            var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);

            var items = queryable
                .Skip(pagedRequest.SkipCount ?? 0)
                .Take(pagedRequest.MaxResultCount ?? 0)
                .ToList();

            if (pagedRequest.Sorting != null)
            {
                items = (ApplyOrderDirection<TEntity>(items.AsQueryable(), pagedRequest.Sorting, pagedRequest.IsAsc ?? false)).ToList();
            }

            return new PagedResult<TEntity>(count, items);
        }

        public async Task<IEnumerable<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate,
            string order,
            bool asc,
            params string[] includes)
        {
            var query = dbset
                .AsNoTracking()
                .Where(predicate)
                .AsQueryable();

            if (includes != null && includes.Any())
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            var items = await query
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (order != null)
            {
                return (ApplyOrderDirection<TEntity>(items.AsQueryable(), order, asc)).ToList();
            }

            return items;
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest<TEntity> pagedRequest,
            Expression<Func<TEntity, bool>> predicate,
            params string[] includes)
        {
            var queryable = dbset
                .AsNoTracking()
                .Where(predicate);

            var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);

            if (includes != null && includes.Any())
            {
                foreach (var item in includes)
                {
                    queryable = queryable.Include(item);
                }
            }

            var items = await queryable
                .Skip(pagedRequest.SkipCount ?? 0)
                .Take(pagedRequest.MaxResultCount ?? 0)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            if (pagedRequest.Sorting != null)
            {
                items = (ApplyOrderDirection<TEntity>(items.AsQueryable(), pagedRequest.Sorting, pagedRequest.IsAsc ?? false)).ToList();
            }

            return new PagedResult<TEntity>(count, items);
        }

        public async Task<PagedResult<TEntity>?> GetListAsync(CancellationToken cancellationToken,
            PagedRequest<TEntity> pagedRequest,
            params string[] includes)
        {
            var queryable = dbset
                .AsNoTracking()
                .AsQueryable();

            var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);

            if (includes != null && includes.Any())
            {
                foreach (var item in includes)
                {
                    queryable = queryable.Include(item);
                }
            }

            var items = await queryable
                .Skip(pagedRequest.SkipCount ?? 0)
                .Take(pagedRequest.MaxResultCount ?? 0)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            if (pagedRequest.Sorting != null)
            {
                items = (ApplyOrderDirection<TEntity>(items.AsQueryable(), pagedRequest.Sorting, pagedRequest.IsAsc ?? false)).ToList();
            }

            return new PagedResult<TEntity>(count, items);
        }

        public async Task<PagedResultV2<TEntity>?> GetListV2Async(CancellationToken cancellationToken, PagedRequestV2 pagedRequest, Expression<Func<TEntity, bool>>? predicate = null)
        {
            var queryable = dbset
                .AsNoTracking()
                .AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);

            queryable = queryable
                .Skip(pagedRequest.Start)
                .Take(pagedRequest.Length);

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var bindings = pagedRequest.GetSelectedColumns().Select(col =>
                Expression.Bind(typeof(TEntity).GetProperty(col),
                Expression.Property(parameter, col)));

            var selector = Expression.Lambda(Expression.MemberInit(Expression.New(typeof(TEntity)), bindings), parameter);

            queryable = queryable.Select((Expression<Func<TEntity, TEntity>>)selector);

            var items = await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);

            if (pagedRequest.Order != null)
            {
                items = (ApplyOrderDirection<TEntity>(items.AsQueryable(), pagedRequest.GetOrderKeys())).ToList();
            }

            return new PagedResultV2<TEntity>(pagedRequest.Draw ?? 0, count, items, pagedRequest.GetOrderKeys());
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

        private IOrderedQueryable<TSource> ApplyOrderDirection<TSource>(IQueryable<TSource> source, List<KeyValuePair<string, bool>>? orders)
        {
            if (orders == null || !orders.Any())
            {
                return source as IOrderedQueryable<TSource>;
            }

            if (orders.Count == 1)
            {
                var f = orders.First();

                var propInfo = typeof(TSource).GetProperty(f.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propInfo == null)
                {
                    throw new ArgumentException("ستون مورد نظر برای مرتب سازی وجود ندارد", f.Key);
                }

                Expression<Func<TSource, object>> orderExp = x => propInfo.GetValue(x, null);

                if (f.Value)
                {
                    return source.OrderBy(orderExp);
                }
                else
                {
                    return source.OrderByDescending(orderExp);
                }
            }

            if (orders.Count == 2)
            {
                var f = orders.First();

                var propInfo = typeof(TSource).GetProperty(f.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propInfo == null)
                {
                    throw new ArgumentException("ستون مورد نظر برای مرتب سازی وجود ندارد", f.Key);
                }

                Expression<Func<TSource, object>> orderExp = x => propInfo.GetValue(x, null);

                if (f.Value)
                {
                    source = source.OrderBy(orderExp);
                }
                else
                {
                    source = source.OrderByDescending(orderExp);
                }

                var l = orders.Last();

                var proplInfo = typeof(TSource).GetProperty(l.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (proplInfo == null)
                {
                    throw new ArgumentException("ستون مورد نظر برای مرتب سازی وجود ندارد", l.Key);
                }

                Expression<Func<TSource, object>> orderlExp = x => proplInfo.GetValue(x, null);

                if (l.Value)
                {
                    return source.OrderBy(orderlExp);
                }
                else
                {
                    return source.OrderByDescending(orderlExp);
                }

            }
            if (orders.Count > 2)
            {
                foreach (var item in orders.Take(orders.Count - 1))
                {
                    var propertyInfo = typeof(TSource).GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new ArgumentException("ستون مورد نظر برای مرتب سازی وجود ندارد", item.Key);
                    }

                    Expression<Func<TSource, object>> orderExpression = x => propertyInfo.GetValue(x, null);

                    if (item.Value)
                    {
                        source = source.OrderBy(orderExpression);
                    }
                    else
                    {
                        source = source.OrderByDescending(orderExpression);
                    }
                }

                var l = orders.Last();

                var proplInfo = typeof(TSource).GetProperty(l.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (proplInfo == null)
                {
                    throw new ArgumentException("ستون مورد نظر برای مرتب سازی وجود ندارد", l.Key);
                }

                Expression<Func<TSource, object>> orderlExp = x => proplInfo.GetValue(x, null);

                if (l.Value)
                {
                    return source.OrderBy(orderlExp);
                }
                else
                {
                    return source.OrderByDescending(orderlExp);
                }
            }

            return source as IOrderedQueryable<TSource>;
        }
    }
}
