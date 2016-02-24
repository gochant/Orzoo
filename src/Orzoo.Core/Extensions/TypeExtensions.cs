using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Orzoo.Core.Extensions
{
    public static class TypeExtensions
    {

        /// <summary>
        /// 获取某个类型的描述名称
        /// </summary>
        /// <param name="type">类的类型</param>
        /// <returns>名称</returns>
        public static string GetDisplayName(this Type type)
        {
            var attributes = (DisplayNameAttribute[])type.GetCustomAttributes(typeof(DisplayNameAttribute), false);

            return attributes.Length == 1 ? attributes[0].DisplayName : type.Name;
        }

        /// <summary>
        /// 获取某个类型的特性值
        /// </summary>
        /// <typeparam name="T">特性的类型</typeparam>
        /// <param name="type">类的类型</param>
        /// <returns>特性</returns>
        public static T GetAttribute<T>(this Type type) where T : class
        {
            var o = type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return (T) o;
        }

        /// <summary>
        /// 获取类型中属性的描述名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyDisplayName(this Type type, string propertyName)
        {
            var attributes = (DisplayAttribute[])type.GetProperty(propertyName)
                .GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes.Length == 1 ? attributes[0].Name : type.Name;
        }

        /// <summary>
        /// 获取类的真实类型（可空类型的基础类型）
        /// </summary>
        /// <param name="type">类的类型</param>
        /// <returns>类的类型</returns>
        public static Type GetRealType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);

            var propertyType = underlyingType ?? type;

            return propertyType;
        }
    }
}