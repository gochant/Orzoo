using System.Data;
using Orzoo.Core.Utility;

namespace Orzoo.Core.Extensions
{
    public static class DataRowExtensions
    {
        /// <summary>
        /// 根据列名获取数据行中单元格的值
        /// </summary>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="row">数据行</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static T CellValue<T>(this DataRow row, string columnName)
        {
            var value = CellValue(row, columnName);
            return ValueHelper.GetSafeValue<T>(value);
        }

        public static object CellValue(this DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                return row[columnName];
            }
            return null;
        }
    }

}
