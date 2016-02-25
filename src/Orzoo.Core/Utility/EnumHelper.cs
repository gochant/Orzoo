using System;
using System.Collections.Generic;
using System.Linq;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Utility
{
    public class EnumHelper
    {
        /// <summary>
        /// 获取枚举类型的所有显示值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>显示值列表</returns>
        public static List<string> GetDisplays<T>() where T : struct
        {
            var t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.GetDisplay()).ToList();
        }

        /// <summary>
        /// 获取枚举的所有描述值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>描述列表</returns>
        public static List<string> GetDescriptions<T>() where T : struct
        {
            var t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.GetDescription()).ToList();
        }

        /// <summary>
        /// 将值转化成对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">值</param>
        /// <returns>枚举</returns>
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
