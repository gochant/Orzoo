﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Orzoo.Core.Data;
using Orzoo.Core.Enums;
using Orzoo.Core.Extensions;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace Orzoo.Core.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PredicateFilter<T>(this IQueryable<T> queryable, DynamicLinqFilterParameter filterArgs)
        {
            if (filterArgs != null && filterArgs.Predicates.Count > 0)
            {
                var predicate = filterArgs.GetCombinedPredicate();

                if (!string.IsNullOrEmpty(predicate))
                {
                    queryable = queryable.Where(predicate, filterArgs.GetCombinedArgs());
                }
            }

            return queryable;
        }

        /// <summary>
        /// 有效数据筛选
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <returns></returns>
        public static IQueryable<T> ValidFilter<T>(this IQueryable<T> queryable) where T : class
        {
            if (typeof(IFlagEntity).IsAssignableFrom(typeof(T)))
            {
                return queryable.Cast<IFlagEntity>()
                .Where(d => !d.Flag.HasValue || d.Flag.Value == DataFlag.Valid)
                .Cast<T>();
            }

            return queryable;
        }

        /// <summary>
        /// 按创建日期降序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <returns></returns>
        public static IQueryable<T> DateOrder<T>(this IQueryable<T> queryable) where T : class
        {
            if (typeof(IMetadataEntity).IsAssignableFrom(typeof(T)))
            {
                return queryable.Cast<IMetadataEntity>().DateOrderStandard().Cast<T>(); // 默认按照创建时间降序排列
            }
            return queryable;
        }

        public static IQueryable<IMetadataEntity> DateOrderStandard(this IQueryable<IMetadataEntity> queryable)
        {
            return queryable.OrderByDescending(d => d.CreatedDate);
        }

        /// <summary>
        /// 将分过组的数据转换成分组结果
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<GroupResult<TKey, TElement>> ToGroupResult<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> list)
        {
            return list.Select(d => new GroupResult<TKey, TElement>
            {
                Value = d.Key,
                Items = d
            });
        }

        public static IQueryable<T> ToDataSourceQueryable<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort, Filter filter, IEnumerable<Aggregator> aggregates)
        {
            // Filter the data first
            queryable = Filter(queryable, filter);

            // Calculate the total number of records (needed for paging)
            var total = queryable.Count();

            // Calculate the aggregates
            var aggregate = Aggregate(queryable, aggregates);

            // Sort the data
            queryable = Sort(queryable, sort);

            // Finally page the data
            if (take > 0)
            {
                queryable = Page(queryable, take, skip);
            }

            return queryable;
        }

        public static IQueryable<T> ToDataSourceQueryable<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort, Filter filter)
        {
            return queryable.ToDataSourceQueryable(take, skip, sort, filter, null);
        }

        public static IQueryable<T> ToDataSourceQueryable<T>(this IQueryable<T> queryable, DataSourceRequest request)
        {
            return queryable.ToDataSourceQueryable(request.Take, request.Skip, request.Sort, request.Filter, null);
        }

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="queryable">查询</param>
        /// <param name="request">请求参数</param>
        /// <returns>参数过滤后的数据查询</returns>
        public static IQueryable<T> ToDataSourceQueryable<T>(this IQueryable<T> queryable, AdvanceDataSourceRequest request)
        {
            request = AdvanceDataSourceRequest.Create(request);
            return queryable.ToDataSourceQueryable(request.Take, request.Skip, request.Sort, request.Filter, request.Aggregates);
        }

        /// <summary>
        /// 生成查询结果
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="queryable">查询</param>
        /// <param name="request">请求参数</param>
        /// <returns>查询结果</returns>
        public static DataSourceResult ToDataSourceResult<T>(this IQueryable<T> queryable, AdvanceDataSourceRequest request)
        {
            request = AdvanceDataSourceRequest.Create(request);
            return queryable.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter, request.Aggregates);
        }

        public static DataSourceResult ToDataSourceResult<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort, Filter filter, IEnumerable<Aggregator> aggregates)
        {
            // Filter the data first
            queryable = Filter(queryable, filter);

            // Calculate the total number of records (needed for paging)
            var total = queryable.Count();

            // Calculate the aggregates
            var aggregate = Aggregate(queryable, aggregates);

            // Sort the data
            queryable = Sort(queryable, sort);

            // Finally page the data
            if (take > 0)
            {
                queryable = Page(queryable, take, skip);
            }

            return new DataSourceResult
            {
                Data = queryable.ToList(),
                Total = total,
                Aggregates = aggregate
            };
        }

        public static DataSourceResult ToDataSourceResult<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort, Filter filter)
        {
            return queryable.ToDataSourceResult(take, skip, sort, filter, null);
        }

	    public static DataSourceResult ToDataSourceResult<T>(this IQueryable<T> queryable, DataSourceRequest request)
        {
            return queryable.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter, null);
        }

        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
        {
            if (filter != null && filter.Logic != null)
            {
                // Collect a flat list of all filters
                var filters = filter.All();

                // Get all filter values as array (needed by the Where method of Dynamic Linq)
                var values = filters.Select(f => f.Value).ToArray();

                // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
                string predicate = filter.ToExpression(filters);

                // Use the Where method of Dynamic Linq to filter the data
                queryable = queryable.Where(predicate, values);
            }

            return queryable;
        }

        private static object Aggregate<T>(IQueryable<T> queryable, IEnumerable<Aggregator> aggregates)
        {
            if (aggregates != null && aggregates.Any())
            {
                var objProps = new Dictionary<DynamicProperty, object>();
                var groups = aggregates.GroupBy(g => g.Field);
                Type type = null;
                foreach (var group in groups)
                {
                    var fieldProps = new Dictionary<DynamicProperty, object>();
                    foreach (var aggregate in group)
                    {
                        var prop = typeof(T).GetProperty(aggregate.Field);
                        var param = Expression.Parameter(typeof(T), "s");
                        var selector = aggregate.Aggregate == "count" && (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                            ? Expression.Lambda(Expression.NotEqual(Expression.MakeMemberAccess(param, prop), Expression.Constant(null, prop.PropertyType)), param)
                            : Expression.Lambda(Expression.MakeMemberAccess(param, prop), param);
                        var mi = aggregate.MethodInfo(typeof(T));
                        if (mi == null)
                            continue;

                        var val = queryable.Provider.Execute(Expression.Call(null, mi,
                            aggregate.Aggregate == "count" && (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                                ? new[] { queryable.Expression }
                                : new[] { queryable.Expression, Expression.Quote(selector) }));

                        fieldProps.Add(new DynamicProperty(aggregate.Aggregate, typeof(object)), val);
                    }
                    type = DynamicExpression.CreateClass(fieldProps.Keys);
                    var fieldObj = Activator.CreateInstance(type);
                    foreach (var p in fieldProps.Keys)
                        type.GetProperty(p.Name).SetValue(fieldObj, fieldProps[p], null);
                    objProps.Add(new DynamicProperty(group.Key, fieldObj.GetType()), fieldObj);
                }

                type = DynamicExpression.CreateClass(objProps.Keys);

                var obj = Activator.CreateInstance(type);

                foreach (var p in objProps.Keys)
                {
                    type.GetProperty(p.Name).SetValue(obj, objProps[p], null);
                }

                return obj;
            }
            else
            {
                return null;
            }
        }

        private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            if (sort != null && sort.Any())
            {
                // Create ordering expression e.g. Field1 asc, Field2 desc
                var ordering = String.Join(",", sort.Select(s => s.ToExpression()));

                // Use the OrderBy method of Dynamic Linq to sort the data
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }

        private static IQueryable<T> Page<T>(IQueryable<T> queryable, int take, int skip)
        {
            return queryable.Skip(skip).Take(take);
        }


    }
}
