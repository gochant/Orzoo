using System.Collections.Generic;
using System.Linq;
using Orzoo.Core.Data;

namespace Orzoo.AspNet.Extensions
{
    public static class QueryableExtensions
    {

        /// <summary>
        /// 对列表进行映射后处理
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<TDto> AfterMap<TModel, TDto>(this IEnumerable<TDto> collection) where TDto : IMapEntity
        {
            var list = collection as IList<TDto> ?? collection.ToList();
            foreach (var item in list)
            {
                item.AfterMap<TModel>();
            }

            return list;
        }

    }
}
