﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Orzoo.Core.Data;
using Orzoo.Core.Extensions;

namespace Orzoo.AspNet.Extensions
{
    public static class DbContextExtensions
    {
        #region Methods

        #region Public Methods

        public static void DeleteAll<T>(this DbContext context) where T : class
        {
            foreach (var p in context.Set<T>())
            {
                context.Entry(p).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// 从代理类获取原始类数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="context">数据上下文</param>
        /// <param name="proxyObject">代理类对象</param>
        /// <returns></returns>
        /// <see cref="http://stackoverflow.com/questions/25770369/get-underlying-entity-object-from-entity-framework-proxy"/>
        public static T UnProxy<T>(this DbContext context, T proxyObject) where T : class
        {
            var proxyCreationEnabled = context.Configuration.ProxyCreationEnabled;
            try
            {
                context.Configuration.ProxyCreationEnabled = false;
                T poco = context.Entry(proxyObject).CurrentValues.ToObject() as T;
                return poco;
            }
            finally
            {
                context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }

        /// <summary>
        /// 获取数据变更记录
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="entity">更改后的数据实体，这个数据实体应该是从数据库里读取出来的才有效</param>
        /// <param name="containsIdProp">是否包含外键Id字段</param>
        /// <returns></returns>
        public static DataChangeLog GetChangeLog(this DbContext context, object entity, bool containsIdProp = true)
        {
            // 记录变更轨迹
            var entry = context.Entry(entity);

            var stateEntry =
                ((IObjectContextAdapter) context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entity);
            var currentValues = stateEntry.CurrentValues;
            var originalValues = stateEntry.OriginalValues;
            //var modifiedProperties = stateEntry.GetModifiedProperties();
            var modifiedProperties =
                entry.CurrentValues.PropertyNames.Where(propertyName => entry.Property(propertyName).IsModified)
                    .ToList();
            var changelog = new DataChangeLog
            {
                DateTime = DateTime.Now,
                Name = entity.GetTypeDisplayName(),
                Type = entity.GetTypeFullName()
            };
            foreach (var prop in modifiedProperties)
            {
                var currentValue = currentValues.GetValue(currentValues.GetOrdinal(prop));
                var originalValue = originalValues.GetValue(originalValues.GetOrdinal(prop));
                if (!originalValue.Equals(currentValue)) // 值已经改变的
                {
                    // 判断是否包含 ID 属性
                    if (containsIdProp || !prop.ToLower().EndsWith("id"))
                    {
                        changelog.DataChangeLogItems.Add(new DataChangeLogItem
                        {
                            Name = entity.GetType().GetPropertyDisplayName(prop),
                            CurrentValue = currentValue,
                            OriginalValue = originalValue
                        });
                    }
                }
            }

            return changelog;
        }

        /// <summary>
        /// 更新数据的时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        public static void UpdateEntityMetadata<T>(this DbContext dbContext)
        {
            // if(entityType.)
            var context = ((IObjectContextAdapter) dbContext).ObjectContext;

            IEnumerable<ObjectStateEntry> objectStateEntries =
                from e in context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified)
                where e.IsRelationship == false && e.Entity is T
                select e;

            var currentTime = DateTime.Now;
            var userName = HttpContext.Current.User?.Identity.Name;

            foreach (var entry in objectStateEntries)
            {
                var entityBase = entry.Entity as IMetadataEntity;

                if (entityBase != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entityBase.CreatedUser = userName;
                        entityBase.CreatedDate = currentTime;
                    }

                    entityBase.ModifiedUser = userName;
                    entityBase.LastModifiedDate = currentTime;
                }
            }
        }

        #endregion

        #endregion
    }
}