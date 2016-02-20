using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.Converter;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Orzoo.Core;
using Orzoo.Core.Utility;

namespace Orzoo.Office
{
    public class ExcelHandler
    {

        /// <summary>
        /// Excel 列名转换为数字
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int ColumnNameToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName");

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += (columnName[i] - 'A' + 1);
            }

            return sum;
        }

        /// <summary>
        /// 数字转Excel列名
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static string NumberToColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        /// <summary>
        /// 单元格名转换为位置
        /// </summary>
        /// <param name="cellname"></param>
        /// <returns></returns>
        public static CellLocationIndex CellNameToLocation(string cellname)
        {
            var match = Regex.Match(cellname, @"([A-Za-z]+)(\d+)");
            var columnName = match.Groups[1].Value;
            var rowIndex = int.Parse(match.Groups[2].Value);
            var columnIndex = ColumnNameToNumber(columnName);
            return new CellLocationIndex() { ColumnIndex = columnIndex - 1, RowIndex = rowIndex - 1 };
        }

        /// <summary>
        /// 位置转换成单元格名
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static string LocationToCellName(int columnIndex, int rowIndex)
        {
            var columnName = NumberToColumnName(columnIndex + 1);
            var rowName = (rowIndex + 1).ToString();
            return columnName + rowName;
        }

        /// <summary>
        /// 位置转换成单元格名
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static string LocationToCellName(CellLocationIndex location)
        {
            return LocationToCellName(location.ColumnIndex, location.RowIndex);
        }


        /// <summary>
        /// 获取Excel版本
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ExcelVersion GetExcelVersion(string filename)
        {
            var ext = Path.GetExtension(filename);
            var version = ExcelVersion.Excel2007;
            switch (ext)
            {
                case ".xls":
                    version = ExcelVersion.Excel2003;
                    break;
                case ".xlsx":
                    version = ExcelVersion.Excel2007;
                    break;
                default:
                    throw new LogicException("不是有效的Excel文件");
            }
            return version;
        }

        public static IWorkbook CreateWorkbook(Stream stream = null, ExcelVersion version = ExcelVersion.Excel2007)
        {
            IWorkbook workbook;
            if (version == ExcelVersion.Excel2003)
            {
                workbook = stream == null ? new HSSFWorkbook() : new HSSFWorkbook(stream);
            }
            else
            {
                workbook = stream == null ? new XSSFWorkbook() : new XSSFWorkbook(stream);
            }
            return workbook;
        }

        public static IClientAnchor CreateClientAnchor(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2, ExcelVersion version = ExcelVersion.Excel2007)
        {
            if (ExcelVersion.Excel2003 == version)
            {
                return new HSSFClientAnchor(dx1, dy1, dx2, dy2, col1, row1, col2, row2);
            }
            else
            {
                return new XSSFClientAnchor(dx1, dy1, dx2, dy2, col1, row1, col2, row2);
            }
        }

        public static IRichTextString CreateRichTextString(string str, ExcelVersion version = ExcelVersion.Excel2007)
        {
            if (ExcelVersion.Excel2003 == version)
            {
                return new HSSFRichTextString(str);
            }
            else
            {
                return new XSSFRichTextString(str);
            }
        }

        public static IComment CreateComment(ISheet sheet, string text, ExcelVersion version)
        {
            var patr = sheet.CreateDrawingPatriarch();
            IComment comment;
            comment = patr.CreateCellComment(CreateClientAnchor(0, 0, 0, 0, 1, 2, 4, 6));
            comment.String = CreateRichTextString(text, version);
            return comment;
        }

        public static ICellStyle CreateErrorStyle(IWorkbook workbook)
        {
            // 设置背景颜色
            var style1 = workbook.CreateCellStyle();
            style1.FillPattern = FillPattern.SolidForeground;
            style1.FillForegroundColor = IndexedColors.Red.Index;
            return style1;
        }

        #region 导入Excel

        public static DataTable ToDataTable(Stream stream, ExcelVersion version = ExcelVersion.Excel2007, int startRow = 0, int startColumn = 0)
        {
            IWorkbook workbook;
            DataTable dt = new DataTable();

            using (stream)
            {
                workbook = CreateWorkbook(stream, version);
            }

            var sheet = workbook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(startRow);
            if (headerRow == null)
            {
                throw new LogicException("数据为空或格式无效的 Excel", AlertType.Warning);
            }
            int cellCount = headerRow.LastCellNum;

            for (int j = startColumn; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1 + startRow); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                if (row == null)
                {
                    break;
                }
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j - row.FirstCellNum] = row.GetCell(j).GetCellValue();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public static DataTable ToDataTable(string filePath, int startRow = 0, int startColumn = 0)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return ToDataTable(stream, GetExcelVersion(filePath), startRow, startColumn);
            }
        }

        public static DataTable ToDataTable(HttpPostedFileBase file, int startRow = 0, int startColumn = 0)
        {
            return ToDataTable(file.InputStream, GetExcelVersion(file.FileName), startRow, startColumn);
        }

        #endregion

        #region 导出Excel

        /// <summary>
        /// 转换成Excel
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="tplFilePath">Excel原始模板路径</param>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="stream">写入的流</param>
        public static void ToExcel(object data, string tplFilePath, string configFilePath, Stream stream)
        {
            var config = XmlHelper.Load<ConfigList<ExcelEntityPropertyMappingConfig>>(configFilePath);
            var mapping = ExcelEntityMapping.GenerateMapping(config, data);
            ToExcel(data, tplFilePath, mapping, stream);
        }

        /// <summary>
        /// 导出Excel到内存流
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="tplFilePath">模板文件路径</param>
        /// <param name="configFilePath">配置文件路径</param>
        /// <returns>内存流</returns>
        public static MemoryStream ToExcel(object data, string tplFilePath, string configFilePath)
        {
            var stream = new MemoryStream();

            ToExcel(data, tplFilePath, configFilePath, stream);

            return stream;
        }

        /// <summary>
        /// 导出Excel到文件
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="tplFilePath">模版文件路径</param>
        /// <param name="destFilePath">生成文件路径</param>
        /// <param name="mapping">映射关系</param>
        public static void ToExcel(object data, string tplFilePath, string destFilePath, ExcelEntityMapping mapping)
        {
            ToExcel(data, tplFilePath, mapping, File.OpenWrite(destFilePath));
        }

        public static void ToExcel(object data, string tplFilePath, ExcelEntityMapping mapping, Stream stream)
        {
            IWorkbook workbook;
            using (var fileStream = File.Open(tplFilePath, FileMode.Open, FileAccess.Read))
            {
                workbook = CreateWorkbook(fileStream, GetExcelVersion(tplFilePath));
            }

            var sheet = workbook.GetSheetAt(0);

            // 解析 mapping

            FillSheet(sheet, data, mapping);

            workbook.Write(stream);
        }


        /// <summary>
        /// 数据表导出Excel
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="stream">导出的流</param>
        /// <param name="errors">将在导出的Excel中设置的数据错误标记</param>
        /// <param name="version">Excel版本</param>
        public static void ToExcel(DataTable dt,
           Stream stream, List<List<KeyValuePair<string, string>>> errors = null, ExcelVersion version = ExcelVersion.Excel2007)
        {
            using (dt)
            {
                var workbook = CreateWorkbook(version: version);
                var sheet = workbook.CreateSheet();
                var headerRow = sheet.CreateRow(0);
                if (errors == null) { errors = new List<List<KeyValuePair<string, string>>>(); }

                foreach (DataColumn column in dt.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                }
                int rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    var dataRow = sheet.CreateRow(rowIndex);
                    // 设置备注
                    var error = errors.ElementAt(rowIndex - 1);

                    foreach (DataColumn column in dt.Columns)
                    {
                        var cell = dataRow.CreateCell(column.Ordinal);

                        cell.SetCellValue(row[column].ToString());

                        // 设置错误信息
                        if (error != null && error.Count != 0)
                        {
                            var items = error.Where(d => d.Key == column.ColumnName);
                            if (items.Count() > 0)
                            {
                                foreach (var item in items)
                                {
                                    cell.CellComment = CreateComment(sheet, item.Key + "：" + item.Value, version);
                                }

                                cell.CellStyle = CreateErrorStyle(workbook);
                            }

                        }

                    }
                    rowIndex++;
                }

                workbook.Write(stream);
                // 由于 NPOI 2007 写入后会自动关闭流，因此不能将此流返回
                //ms.Flush();
                //ms.Position = 0;
            }
            //return ms;
        }

        /// <summary>
        /// 填充工作表
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="data">数据</param>
        /// <param name="mapping">映射</param>
        protected static void FillSheet(ISheet sheet, object data, ExcelEntityMapping mapping)
        {
            foreach (var propMapping in mapping)
            {
                object value;
                if (propMapping.PropertyName == null)
                {
                    value = propMapping.Value;
                    var cell = sheet.GetCell(propMapping.CellName);
                    cell.SetCellValue(value.ToString());
                }
                else
                {
                    var propertyInfo = data.GetType().GetProperty(propMapping.PropertyName);
                    value = propertyInfo.GetValue(data);
                    if (value == null)
                    {
                        value = propMapping.Value;
                    }

                    if (propMapping.Items != null && propMapping.Items.Count != 0)
                    {
                        var listValue = (IList)value;

                        for (int i = 0; i < propMapping.Items.Count; i++)
                        {
                            var itemMapping = propMapping.Items[i];
                            // 找到对应的数据
                            var itemData = listValue[i];

                            FillSheet(sheet, itemData, itemMapping);
                        }
                    }
                    else
                    {
                        var cell = sheet.GetCell(propMapping.CellName);
                        var type = propertyInfo.PropertyType;
                        cell.SetCellValue(value, type);
                    }
                }

            }
        }


        public static string ToHtml(byte[] xlsBytes, ExcelVersion version)
        {
            var converter = new ExcelToHtmlConverter
            {
                OutputColumnHeaders = false,
                OutputHiddenColumns = false,
                OutputHiddenRows = false,
                OutputLeadingSpacesAsNonBreaking = true,
                OutputRowNumbers = false,
                UseDivsToSpan = false
            };


            var workbook = CreateWorkbook(new MemoryStream(xlsBytes), version);

            converter.ProcessWorkbook(workbook);

            var outStream = new MemoryStream();

            converter.Document.Save(outStream);

            return Encoding.UTF8.GetString(outStream.ToArray());
        }

        //public static string ToPdf(byte[] xlsBytes, ExcelVersion version)
        //{
        //    var workbook = CreateWorkbook(new MemoryStream(xlsBytes), version);

        //    workbook.
        //}

        #endregion

    }

}
