using System;
using System.Collections.Generic;
using System.Linq;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Utility
{
    public class EnumHelper
    {
        public static List<string> GetDisplays<T>() where T : struct
        {
            var t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.GetDisplay()).ToList();
        }

        public static List<string> GetDescriptions<T>() where T : struct
        {
            var t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.GetDescription()).ToList();
        }

        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
