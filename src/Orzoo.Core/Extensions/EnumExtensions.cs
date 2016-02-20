using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举值的显示名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplay(this Enum value)
        {
            var info = value.GetType().GetField(value.ToString());

            var attributes = (DisplayAttribute[])info.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes.Length == 1)
            {
                return attributes[0].Name;
            }
            return value.ToString();
        }

        /// <summary>
        /// 获取枚举值描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}