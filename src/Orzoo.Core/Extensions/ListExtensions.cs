using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Orzoo.Core.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// 转换成 DataTable
        /// </summary>
        /// <typeparam name="T">列表数据类型</typeparam>
        /// <param name="data">列表数据</param>
        /// <returns>数据表</returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            var values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }


    }
}