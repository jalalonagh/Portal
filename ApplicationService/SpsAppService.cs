//using Core;
//using DataTransferObjects;
//using JO.AutoMapper;
//using JO.Data.Base;
//using JO.Data.Base.Interfaces;
//using JO.Response.Base;
//using System.Linq.Expressions;

//namespace ApplicationService
//{
//    public class JOAppService<TEntity> : IJOAppService<TEntity>
//        where TEntity : class, IJOEntity
//    {
//        protected IJOMapper mapper;
//        protected DomainManager<TEntity> manager;
//        protected IEfCoreRepository<TEntity> repo;

//        public JOAppService(IJOMapper _mapper, IEfCoreRepository<TEntity> _repo, DomainManager<TEntity> _manager)
//        {
//            this.mapper = _mapper;
//            this.repo = _repo;
//            this.manager = _manager;
//        }

//        #region Insert
//        public async Task<VM?> InsertAsync<VM>(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
//            where VM : BaseViewModel
//        {
//            entity.Validate();

//            var result = await manager.InsertAsync(cancellationToken, entity, autoSave);

//            return mapper.Map<VM?>(result);
//        }

//        public async Task<IEnumerable<VM>?> InsertManyAsync<VM>(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = true)
//            where VM : BaseViewModel
//        {
//            foreach (var item in entities)
//                item.Validate();

//            var result = await manager.InsertManyAsync(cancellationToken, entities, autoSave);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }
//        #endregion

//        #region Edit
//        public async Task<VM?> UpdateAsync<VM>(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
//            where VM : BaseViewModel
//        {
//            entity.Validate();

//            var result = await manager.UpdateAsync(cancellationToken, entity, autoSave);

//            return mapper.Map<VM?>(result);
//        }

//        public async Task<IEnumerable<VM>?> UpdateManyAsync<VM>(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = true)
//            where VM : BaseViewModel
//        {
//            foreach (var item in entities)
//                item.Validate();

//            var result = await manager.UpdateManyAsync(cancellationToken, entities, autoSave);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }
//        #endregion

//        #region Delete
//        public async Task<List<VM>?> DeleteDirectAsync<VM>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predict, bool autoSave = true)
//        {
//            var result = await manager.DeleteDirectAsync(cancellationToken, predict, autoSave);

//            return mapper.Map<List<VM>?>(result);
//        }

//        public async Task<VM?> DeleteAsync<VM>(CancellationToken cancellationToken, TEntity entity, bool autoSave = true)
//            where VM : BaseViewModel
//        {
//            entity.Validate();

//            var result = await manager.DeleteAsync(cancellationToken, entity, autoSave);

//            return mapper.Map<VM?>(result);
//        }

//        public async Task<IEnumerable<VM>?> DeleteManyAsync<VM>(CancellationToken cancellationToken, IEnumerable<TEntity> entities, bool autoSave = false)
//            where VM : BaseViewModel
//        {
//            foreach (var item in entities)
//                item.Validate();

//            var result = await manager.DeleteManyAsync(cancellationToken, entities, autoSave);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }
//        #endregion

//        #region Find
//        public async Task<VM?> FindAsync<VM>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
//            where VM : BaseViewModel
//        {
//            var result = await repo.FindAsync(cancellationToken, predicate);

//            return mapper.Map<VM?>(result);
//        }

//        public async Task<VM?> FindAsync<VM>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate,
//            params string[] includes)
//            where VM : BaseViewModel
//        {
//            var result = await repo.FindAsync(cancellationToken, predicate, includes);

//            return mapper.Map<VM?>(result);
//        }
//        #endregion

//        #region List
//        public async Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }

//        public async Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken, params string[] includes)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, includes);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }

//        public async Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken, string order, bool asc)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken: cancellationToken, order, asc);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }

//        public async Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, predicate);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }

//        public async Task<PagedResultVM<VM>?> GetListAsync<VM>(CancellationToken cancellationToken, PagedRequest<TEntity> pagedRequest)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, pagedRequest);

//            return new PagedResultVM<VM>(result.Count, mapper.Map<List<VM>?>(result.Items));
//        }

//        public async Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            Expression<Func<TEntity, bool>> predicate,
//            string order,
//            bool asc)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, predicate, order, asc);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }

//        public async Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            Expression<Func<TEntity, bool>> predicate,
//            params string[] includes)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, predicate, includes);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }

//        public async Task<PagedResultVM<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            PagedRequest<TEntity> pagedRequest,
//            Expression<Func<TEntity, bool>> predicate)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, pagedRequest, predicate);

//            return new PagedResultVM<VM>(result.Count, mapper.Map<List<VM>?>(result.Items));
//        }

//        public async Task<IEnumerable<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            Expression<Func<TEntity, bool>> predicate,
//            string order,
//            bool asc,
//            params string[] includes)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, predicate, order, asc, includes);

//            return mapper.Map<IEnumerable<VM>?>(result);
//        }

//        public async Task<PagedResultVM<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            PagedRequest<TEntity> pagedRequest,
//            Expression<Func<TEntity, bool>> predicate,
//            params string[] includes)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, pagedRequest, predicate, includes);

//            return new PagedResultVM<VM>(result.Count, mapper.Map<List<VM>?>(result.Items));
//        }

//        public async Task<PagedResultVM<VM>?> GetListAsync<VM>(CancellationToken cancellationToken,
//            PagedRequest<TEntity> pagedRequest,
//            params string[] includes)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListAsync(cancellationToken, pagedRequest, includes);

//            return new PagedResultVM<VM>(result.Count, mapper.Map<List<VM>?>(result.Items));
//        }

//        public async Task<PagedResultV2VM<VM>?> GetListV2Async<VM>(CancellationToken cancellationToken,
//            PagedRequestV2 pagedRequest,
//            Expression<Func<TEntity, bool>>? predicate = null)
//            where VM : BaseViewModel
//        {
//            var result = await repo.GetListV2Async(cancellationToken,
//                new PagedRequestV2(pagedRequest.Draw ?? 0,
//                pagedRequest.Start,
//                pagedRequest.Length,
//                pagedRequest.Columns,
//                pagedRequest.AllowedColumns,
//                pagedRequest.Order,
//                pagedRequest.Search),
//                predicate);

//            var vm = mapper.Map<List<VM>>(result.Data);

//            return new PagedResultV2VM<VM>()
//            {
//                Draw = pagedRequest.Draw ?? 0,
//                OrderColumnName = result.OrderColumnName,
//                RecordsFiltered = result.RecordsFiltered,
//                RecordsTotal = result.RecordsTotal,
//                Data = vm,
//            };
//        }
//        #endregion
//    }
//}
