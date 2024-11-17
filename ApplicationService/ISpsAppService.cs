//using DataTransferObjects;
//using JO.Data.Base;
//using JO.Response.Base;
//using System.Linq.Expressions;

//namespace ApplicationService
//{
//    public interface IJOAppService<TEntity> : IAppService
//        where TEntity : class, IJOEntity
//    {
//        Task<VM?> InsertAsync<VM>(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> InsertManyAsync<VM>(CancellationToken cancellationToken,
//            IEnumerable<TEntity> entities,
//            bool autoSave = true)
//            where VM : BaseViewModel;

//        Task<VM?> UpdateAsync<VM>(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> UpdateManyAsync<VM>(CancellationToken cancellationToken,
//            IEnumerable<TEntity> entities,
//            bool autoSave = true)
//            where VM : BaseViewModel;

//        Task<VM?> DeleteAsync<VM>(CancellationToken cancellationToken, TEntity entity, bool autoSave = false)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> DeleteManyAsync<VM>(CancellationToken cancellationToken,
//            IEnumerable<TEntity> entities,
//            bool autoSave = true)
//            where VM : BaseViewModel;

//        Task<VM?> FindAsync<VM>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
//            where VM : BaseViewModel;

//        Task<VM?> FindAsync<VM>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate,
//            params string[] includes)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken, params string[] includes)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken, string order, bool asc)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
//            where VM : BaseViewModel;

//        Task<PagedResultVM<VM>?> GetListAsync<VM>(CancellationToken cancellationToken, PagedRequest<TEntity> pagedRequest)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            Expression<Func<TEntity, bool>> predicate,
//            string order,
//            bool asc)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            Expression<Func<TEntity, bool>> predicate,
//            params string[] includes)
//            where VM : BaseViewModel;

//        Task<PagedResultVM<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            PagedRequest<TEntity> pagedRequest,
//            Expression<Func<TEntity, bool>> predicate)
//            where VM : BaseViewModel;

//        Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            Expression<Func<TEntity, bool>> predicate,
//            string order,
//            bool asc,
//            params string[] includes)
//            where VM : BaseViewModel;

//        Task<PagedResultVM<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            PagedRequest<TEntity> pagedRequest,
//            Expression<Func<TEntity, bool>> predicate,
//            params string[] includes)
//            where VM : BaseViewModel;
//        Task<PagedResultVM<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            PagedRequest<TEntity> pagedRequest,
//            params string[] includes)
//            where VM : BaseViewModel;

//        Task<List<VM>?> DeleteDirectAsync<VM>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predict, bool autoSave = true);
//        Task<PagedResultV2VM<VM>?> GetListV2Async<VM>(CancellationToken cancellationToken, PagedRequestV2 pagedRequest, Expression<Func<TEntity, bool>>? predicate = null) where VM : BaseViewModel;
//    }
//}
