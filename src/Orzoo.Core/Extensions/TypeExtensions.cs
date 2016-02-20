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
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Type type)
        {
            var attributes = (DisplayNameAttribute[])type.GetCustomAttributes(typeof(DisplayNameAttribute), false);

            return attributes.Length == 1 ? attributes[0].DisplayName : type.Name;
        }

        public static T GetAttribute<T>(this Type type) where T : class
        {
            var o = type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return o == null ? null : (T)o;
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

        public static Type GetRealType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);

            var propertyType = underlyingType ?? type;

            return propertyType;
        }
    }
}