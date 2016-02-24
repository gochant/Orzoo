using System.Linq;
using System.Reflection;

namespace Orzoo.Core.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 获取某个属性的Attribute
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="property">属性对象</param>
        /// <returns>特性</returns>
        public static T GetAttribute<T>(this PropertyInfo property)
        {
            var attr = (T)property.GetCustomAttributes(typeof(T), true).SingleOrDefault();
            return attr;
        }
    }
}
