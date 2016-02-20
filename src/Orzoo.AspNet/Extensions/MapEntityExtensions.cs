using AutoMapper;
using Orzoo.Core.Data;

namespace Orzoo.AspNet.Extensions
{
    public static class MapEntityExtensions
    {
        public static void AfterMap<T>(this IMapEntity entity)
        {
            var map = Mapper.FindTypeMapFor(typeof(T), entity.GetType());

            map.AfterMap(null, entity);
        }

        /// <summary>
        /// 保护数据
        /// </summary>
        /// <param name="entity">实体本身</param>
        /// <param name="protectedFields">要保护的字段</param>
        /// <returns></returns>
        public static object ProtectData(this object entity, string[] protectedFields)
        {
            var type = entity.GetType();
            foreach (var prop in protectedFields)
            {
                type.GetProperty(prop)?.SetValue(entity, null);
            }

            return entity;
        }
    }
}