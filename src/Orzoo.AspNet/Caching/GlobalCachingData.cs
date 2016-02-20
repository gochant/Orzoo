using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Orzoo.AspNet.Caching
{
    /// <summary>
    /// 全局数据缓存
    /// </summary>
    public class GlobalCachingData : GlobalCachingProvider
    {
        #region Properties, Indexers

        /// <summary>
        /// DbContext 类型
        /// </summary>
        public Type DbContextType { get; set; }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 缓存初始化
        /// </summary>
        /// <param name="type">DbContext的类型</param>
        /// <returns></returns>
        public GlobalCachingData Init(Type type)
        {
            DbContextType = type;
            return this;
        }

        /// <summary>
        /// 缓存实体所有数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public object Cache<TEntity>(Func<DbContext, object> fetchData = null, bool lazyLoading = false)
            where TEntity : class
        {
            var key = typeof (TEntity).Name;
            if (fetchData == null)
            {
                fetchData = context => context.Set<TEntity>().ToList();
            }
            return Cache(key, fetchData, lazyLoading);
        }

        public object Cache(string key, Func<DbContext, object> fetchData, bool lazyLoading = false)
        {
            using (var context = (DbContext) DependencyResolver.Current.GetService(DbContextType))
            {
                context.Configuration.ProxyCreationEnabled = false; // 禁用代理类
                context.Configuration.LazyLoadingEnabled = lazyLoading; // 禁用懒加载
                var data = fetchData(context);
                Instance.AddItem(key, data);
                return data;
            }
        }

        /// <summary>
        /// 获取实体所有数据
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <returns></returns>
        public object Get<TEntity>() where TEntity : class
        {
            var name = typeof (TEntity).Name;
            var result = Instance.GetItem(name) ?? Cache<TEntity>();
            return result;
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public List<TEntity> GetList<TEntity>() where TEntity : class
        {
            var obj = Get<TEntity>();
            return (obj as List<TEntity>) ?? new List<TEntity>();
        }

        #endregion

        #endregion

        #region Singleton 

        protected GlobalCachingData()
        {
        }

        /// <summary>
        /// 缓存实例
        /// </summary>
        public new static GlobalCachingData Instance => Nested.instance;

        private class Nested
        {
            #region Static Fields and Constants

            internal static readonly GlobalCachingData instance = new GlobalCachingData();

            #endregion

            #region Constructors

            // 正确设置静态构造器告诉 C# 编译器
            // 不要将类型标记为 beforefieldinit
            static Nested()
            {
            }

            #endregion
        }

        #endregion
    }
}