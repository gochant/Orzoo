using System;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Utility
{
    public class ValueHelper
    {
        /// <summary>
        /// 获取一个安全类型的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetSafeValue(object value, Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            var safeValue = (value == null) ? underlyingType.GetDefault() : Convert.ChangeType(value, underlyingType);
            return safeValue;
        }

        public static T GetSafeValue<T>(object value)
        {
            var type = typeof(T);
            var t = Nullable.GetUnderlyingType(type) ?? type;
            return (value == null) ? default(T) : (T)Convert.ChangeType(value, t);
        }

        /// <summary>
        /// 获取类型的Js类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetJsType(Type type)
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
                    break;
            }
            return jsTypes[index];
        }
    }

}
