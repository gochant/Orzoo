using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Orzoo.AspNet.Caching;
using Orzoo.AspNet.Extensions;
using Orzoo.Core;
using Orzoo.Core.Data;
using Orzoo.Core.Enums;
using Orzoo.Core.Extensions;
using Orzoo.Core.Linq;

namespace Orzoo.AspNet.Infrastructure
{
    public class BaseService<T, TKey> : IDisposable, IService, ICacheService<T>
        where T : class, IEntity<TKey>, new()
    {
        #region Constructors

        public BaseService(DbContext context)
        {
            Context = context;
            DbEntitySet = context.Set<T>();
        }

        #endregion

        #region Properties, Indexers

        public DbContext Context { get; private set; }


        protected DbSet<T> DbEntitySet { get; private set; }

        #endregion

        #region Methods

        #region Protected Methods

        protected virtual async Task<Feedback> InnerModifyOrCreateSave(T entity, EditMode mode,
            Func<T, Task<Feedback>> logic, bool isForce)
        {
            entity = PreprocessBeforeSave(entity, mode);

            return await InnerEditSave(entity, entity, mode, logic, isForce);
        }

        protected virtual async Task<Feedback> InnerEditSave(T entity, T temp, EditMode mode,
            Func<T, Task<Feedback>> logic, bool isForce)
        {
            var result = Feedback.CreateFail();
            var canBeEdited = isForce || CanBeEdited(entity, mode);

            if (canBeEdited)
            {
                result = await logic(entity);
                UpdateCache();
            }

            result.Temp["entity"] = temp; // 将当前实体添加到 temp 结构中
            return result;
        }

        protected virtual async Task<Feedback> InnerDeleteSave(TKey id, Func<T, Task<Feedback>> logic, bool isForce)
        {
            if (id == null)
            {
                throw new LogicException("未传入删除的键值");
            }

            var data = await GetByIdAsync(id);
            if (data == null)
            {
                throw new LogicException("未找到数据");
            }

            var temp = Context.UnProxy(data);

            return await InnerEditSave(data, temp, EditMode.Delete, logic, isForce);
        }


        /// <summary>
        /// 检测数据能否被编辑
        /// </summary>
        /// <param name="data">待编辑的数据</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        protected virtual bool CanBeEdited(T data, EditMode mode)
        {
            return true;
        }


        /// <summary>
        /// 保存数据前的预处理（可用于数据填充和验证）
        /// </summary>
        /// <param name="data">待保存的数据</param>
        /// <param name="mode">编辑模式</param>
        /// <returns></returns>
        protected virtual T PreprocessBeforeSave(T data, EditMode mode)
        {
            return data;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取数据查询
        /// </summary>
        /// <param name="queryable">传入查询</param>
        /// <param name="sorted">是否排序</param>
        /// <param name="noTracking">是否跟踪</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetQuery(IQueryable<T> queryable = null, bool sorted = true, bool noTracking = true)
        {
            if (queryable == null)
            {
                queryable = noTracking ? DbEntitySet.AsNoTracking() : DbEntitySet;
                if (typeof (IFlagEntity).IsAssignableFrom(typeof (T)))
                {
                    queryable = queryable.ValidFilter();
                }

                if (sorted)
                {
                    if (typeof (IMetadataEntity).IsAssignableFrom(typeof (T)))
                    {
                        queryable = queryable.Cast<IMetadataEntity>().DateOrderStandard().Cast<T>(); // 默认按照创建时间降序排列
                    }
                    else
                    {
                        // 默认按照Id排序
                        queryable = queryable.OrderBy(d => d.Id);
                    }
                }
            }
            return queryable;
        }

        /// <summary>
        /// 获取列表结果结果
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="queryable">查询</param>
        /// <returns></returns>
        public virtual DataSourceResult GetListResult<TDto>(AdvanceDataSourceRequest request,
            IQueryable<T> queryable = null)
            where TDto : class, IMapEntity
        {
            var result = GetListResult<T, TDto>(GetQuery(queryable), request);
            return result;
        }

        public virtual DataSourceResult GetListResult<TEntity, TEntityDto>(IQueryable<TEntity> queryable,
            AdvanceDataSourceRequest request)
            where TEntityDto : class, IMapEntity
        {
            var result = queryable.ProjectTo<TEntityDto>().ToDataSourceResult(request);
            result.Data = ProcessListResult<TEntity, TEntityDto>((IEnumerable<TEntityDto>) result.Data);

            if (request.Fields.Count > 0)
            {
                var fieldsString = string.Join(",", request.Fields);
                result.Data = result.Data.AsQueryable().Select($"new ({fieldsString})");
            }

            return result;
        }


        /// <summary>
        /// 处理列表结果
        /// </summary>
        /// <typeparam name="TEntity">数据库实体</typeparam>
        /// <typeparam name="TEntityDto">传输数据实体</typeparam>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntityDto> ProcessListResult<TEntity, TEntityDto>(IEnumerable<TEntityDto> list)
            where TEntityDto : class, IMapEntity
        {
            // 后映射
            return list.AfterMap<TEntity, TEntityDto>();
        }

        /// <summary>
        /// 处理单个实体结果
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEntityDto"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual TEntityDto ProcessEntityResult<TEntity, TEntityDto>(TEntityDto dto)
        {
            if (dto != null)
            {
                ((IMapEntity) dto).AfterMap<TEntity>();
            }

            return dto;
        }

        public virtual Task<T> GetByIdAsync(TKey id)
        {
            //if (query == null) { query = DbEntitySet; }
            // return query.FirstOrDefaultAsync(d => d.Id.ToString() == id.ToString());
            return DbEntitySet.FindAsync(id);
        }

        public virtual TDto GetById<TDto>(TKey id) where TDto : IEntity<TKey>, IDto
        {
            return GetByQueryable<TDto>(GetQuery(sorted: false).AsNoTracking().Where("Id == @0", id));
        }

        public virtual async Task<TDto> GetByIdAsync<TDto>(TKey id)
            where TDto : IEntity<TKey>, IDto
        {
            return await GetByQueryableAsync<TDto>(GetQuery(sorted: false).Where("Id == @0", id));
        }

        public virtual TDto GetByQueryable<TDto>(IQueryable<T> queryable)
        {
            var dto = queryable.ProjectTo<TDto>().FirstOrDefault();
            ProcessEntityResult<T, TDto>(dto);
            return dto;
        }

        public virtual async Task<TDto> GetByQueryableAsync<TDto>(IQueryable<T> queryable)
        {
            var dto = await queryable.ProjectTo<TDto>().FirstOrDefaultAsync();
            ProcessEntityResult<T, TDto>(dto);
            return dto;
        }

        /// <summary>
        /// 获取某个实体，在内存中进行映射
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TDto> GetByIdAsyncMemoryMapping<TDto>(TKey id) where TDto : IEntity<TKey>, IDto
        {
            var data = await DbEntitySet.FindAsync(id);
            if (data != null)
            {
                var dto = Mapper.Map<T, TDto>(data);
                return dto;
            }

            return default(TDto);
        }

        public virtual Task<T> GetByIdNoTrackingAsync(object id)
        {
            return DbEntitySet.AsNoTracking().FirstOrDefaultAsync(d => d.Id.ToString() == id.ToString());
        }

        public virtual T GetByIdNoTracking(object id)
        {
            return DbEntitySet.AsNoTracking().FirstOrDefault(d => d.Id.ToString() == id.ToString());
        }


        public virtual T GetById(object id)
        {
            return DbEntitySet.Find(id);
        }

        /// <summary>
        /// 根据IDs 获取对象（会乱序）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetByIds(IList<TKey> ids, IQueryable<T> query = null)
        {
            if (query == null)
            {
                query = DbEntitySet;
            }
            return query.Where(r => ids.Contains(r.Id));
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="save">是否提交数据库保存</param>
        /// <returns></returns>
        public virtual async Task<DbResult> CreateAsync(T entity, bool save = true)
        {
            DbEntitySet.Add(entity);
            return save ? await SaveChangesAsync() : null;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public virtual async Task<DbResult> DeleteAsync(T entity)
        {
            if (entity != null)
            {
                DbEntitySet.Remove(entity);
                return await SaveChangesAsync();
            }

            return DbResult.Failed();
        }

        public virtual async Task<DbResult> DeleteRangeAsync(IEnumerable<T> list)
        {
            DbEntitySet.RemoveRange(list);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 删除（根据ID）
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public virtual async Task<DbResult> DeleteAsync(TKey id)
        {
            var item = await GetByIdAsync(id);
            return await DeleteAsync(item);
        }

        public virtual async Task<DbResult> UpdateFlagAsync(TKey id, DataFlag flag)
        {
            var item = await GetByIdAsync(id);
            if (typeof (IFlagEntity).IsAssignableFrom(typeof (T)))
            {
                ((IFlagEntity)item).Flag = flag;
            }
            return await UpdateAsync(item);
        }

        public virtual async Task<DbResult> UpdateFlagAsync(T item, DataFlag flag)
        {
            if (typeof(IFlagEntity).IsAssignableFrom(typeof(T)))
            {
                ((IFlagEntity)item).Flag = flag;
            }
            return await UpdateAsync(item);
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="save">是否提交数据库保存</param>
        /// <returns></returns>
        public virtual async Task<DbResult> UpdateAsync(T entity, bool save = true)
        {
            if (entity != null)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return save ? await SaveChangesAsync() : null;
            }
            return DbResult.NullFailed(typeof (T).GetDisplayName());
        }

        public DbResult SaveChanges()
        {
            if (!HasUnsavedChanges()) return DbResult.Success;
            Context.UpdateEntityMetadata<T>();
            // TriggerChangeEvent();
            return new DbResult().From(Context.SaveChanges());
        }

        public async Task<DbResult> SaveChangesAsync()
        {
            if (!HasUnsavedChanges()) return DbResult.Success;

            Context.UpdateEntityMetadata<T>();
            return new DbResult().From(await Context.SaveChangesAsync());
        }

        public virtual async Task<Feedback> CreateSave(T data, bool isForce)
        {
            return
                await
                    InnerModifyOrCreateSave(data, EditMode.Create,
                        async entity => (await CreateAsync(entity)).ToFeedback(), isForce);
        }

        public virtual async Task<Feedback> ModifySave(T data, bool isForce)
        {
            return await InnerModifyOrCreateSave(data, EditMode.Modify,
                async entity => (await UpdateAsync(entity)).ToFeedback(), isForce);
        }

        public virtual async Task<Feedback> DeleteSave(TKey id, bool isForce)
        {
            return await InnerDeleteSave(id, async entity => (await DeleteAsync(entity)).ToFeedback(), isForce);
        }

        /// <summary>
        /// 安全的删除（只改数据有效性标志位）
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="isForce">是否强制删除</param>
        /// <returns></returns>
        public virtual async Task<Feedback> SafeDeleteSave(TKey id, bool isForce)
        {
            return
                await
                    InnerDeleteSave(id, async entity => (await UpdateFlagAsync(entity, DataFlag.Invalid)).ToFeedback(),
                        isForce);
        }


        /// <summary>
        /// 创建新的实体
        /// </summary>
        /// <returns></returns>
        public virtual T CreateEntity(T data)
        {
            return Mapper.Map(data, new T());
        }

        public virtual TDto CreateEntity<TDto>(T data)
        {
            var entity = CreateEntity(data);
            var result = Mapper.Map<T, TDto>(entity);
            return result;
        }

        /// <summary>
        /// 有未保存的更改
        /// </summary>
        /// <returns></returns>
        public bool HasUnsavedChanges()
        {
            //return Context.ChangeTracker.Entries().Any(e => e.State == EntityState.Added
            //                                          || e.State == EntityState.Modified
            //                                          || e.State == EntityState.Deleted);
            return true;
        }

        #endregion

        #endregion

        #region ICacheService<T> Members

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        public bool UseCache { get; set; } = false;

        public virtual List<T> GetCache()
        {
            return GlobalCachingData.Instance.Get<T>() as List<T>;
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        public virtual void UpdateCache()
        {
            if (UseCache)
            {
                GlobalCachingData.Instance.Cache<T>();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Context.Dispose();
        }

        #endregion
    }
}