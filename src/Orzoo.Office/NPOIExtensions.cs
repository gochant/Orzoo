using System;
using NPOI.SS.UserModel;

namespace Orzoo.Office
{
    public static class NPOIExtensions
    {
        /// <summary>
        /// 获取Cell
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="xlsIndex">cell的索引，例 "E5"</param>
        /// <param name="createIt">是否在cell不存在时创建它</param>
        /// <returns></returns>
        public static ICell GetCell(this ISheet sheet, string xlsIndex, bool createIt = true)
        {
            var xy = ExcelHandler.CellNameToLocation(xlsIndex);
            var row = sheet.GetRow(xy.RowIndex);
            if (row == null && createIt)
            {
                row = sheet.CreateRow(xy.RowIndex);
            }
            if (row != null)
            {
                var cell = row.GetCell(xy.ColumnIndex);
                if (cell == null && createIt)
                {
                    cell = row.CreateCell(xy.ColumnIndex);
                }
                return cell;
            }
            return null;
        }

        /// <summary>
        /// 设置 Cell 的值
        /// </summary>
        /// <param name="cell">当前cell</param>
        /// <param name="value">值</param>
        /// <param name="type">值类型</param>
        /// <remarks>因为 NPOI 设置的值必须是个特定类型，因此必须提供特定的值</remarks>
        public static void SetCellValue(this ICell cell, object value, Type type = null)
        {
            if (value == null) return;
            if (type == null) { type = value.GetType(); }

            var t = Nullable.GetUnderlyingType(type) ?? type;  // 获取实际类型
            var safeValue = Convert.ChangeType(value, t);

            if (t == typeof(DateTime))
            {
                cell.SetCellValue((DateTime)safeValue);
                return;
            }
            if (t == typeof(double))
            {
                cell.SetCellValue((double)safeValue);
                return;
            }
            if (t == typeof(decimal))
            {
                cell.SetCellValue((double)(decimal)safeValue);
                return;
            }
            if (t == typeof(int))
            {
                cell.SetCellValue((int)safeValue);
                return;
            }
            if (t == typeof(string))
            {
                cell.SetCellValue((string)safeValue);
                return;
            }
            if (t == typeof(bool))
            {
                cell.SetCellValue((bool)safeValue);
                return;
            }

            // 默认转换成字符串设置
            cell.SetCellValue(safeValue.ToString());
        }


        public static object GetCellValue(this ICell cell)
        {
            var cellType = cell.CellType == CellType.Formula ? cell.CachedFormulaResultType : cell.CellType;

            if(cellType == CellType.String)
            {
                return cell.StringCellValue;
            }
            if (cellType == CellType.Numeric)
            {
                if(DateUtil.IsCellDateFormatted(cell))
                {
                    return cell.DateCellValue;
                }
                return cell.NumericCellValue;
            }

            if(cellType == CellType.Boolean)
            {
                return cell.BooleanCellValue;
            }

            if(cellType == CellType.Blank)
            {
                return null;
            }

            return cell.StringCellValue;
        }
    }

}
