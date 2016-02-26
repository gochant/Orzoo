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
            return (T)o;
        }

        /// <summary>
        /// 获取类型中属性的描述名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyDisplayName(this Type type, string propertyName)
        {
            var attributes = (DisplayAttribute[])type.GetProperty(propertyName)?
                .GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes?.Length == 1 ? attributes[0].Name : null;
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

        /// <summary>
        /// 获取类型的Js类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetJsType(this Type type)
        {
            var typename = type.GetRealType()?.Name;
            var jsTypes = new[] { "string", "date", "number", "boolean", "array", "object" };
            var index = 0;
            switch (typename)
            {
                case "String":
                    index = 0;
                    break;
                case "DateTime":
                    index = 1;
                    break;
                case "Double":
                case "Int32":
                case "Decimal":
                case "Float":
                    index = 2;
                    break;
                case "Boolean":
                    index = 3;
                    break;
                default:
                    index = 5;
                    break;
            }
            return jsTypes[index];
        }

        /// <summary>
        /// 获取类型的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}