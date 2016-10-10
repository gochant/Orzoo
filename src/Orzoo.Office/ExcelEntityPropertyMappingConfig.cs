using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Orzoo.Office.Utility;

namespace Orzoo.Office
{
    /// <summary>
    /// Excel 实体数据映射配置
    /// </summary>
    [XmlType("mapping")]
    public class ExcelEntityPropertyMappingConfig
    {
        /// <summary>
        /// 实体内的属性名
        /// </summary>
        [XmlAttribute("prop")]
        public string PropertyName { get; set; }

        /// <summary>
        /// cell 索引
        /// </summary>
        [XmlAttribute("cell")]
        public string CellName { get; set; }

        /// <summary>
        /// 列表可用：列表方向
        /// </summary>
        [XmlIgnore]
        public RenderDirection? Direction { get; set; }

        /// <summary>
        /// 列表可用：列表数据呈现时，行列数据限制，超过则换行列
        /// </summary>
        [XmlAttribute("limit")]
        public string LineLimit { get; set; }

        /// <summary>
        /// 直接设置到 cell 上的值（不通过 PropertyName读取）
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }

        /// <summary>
        /// 列表可用：列表列的增长值，例如：如果是1，则数据列紧挨着渲染
        /// </summary>
        [XmlAttribute("increase")]
        public string Increase { get; set; }

        /// <summary>
        /// 列表可用：列表中每一列的设置
        /// </summary>
        [XmlElement("mapping")]
        public List<ExcelEntityPropertyMappingConfig> ItemProperties { get; set; }

        #region 序列化控制 

        //public bool ShouldSerializeXmlLineLimit() { return LineLimit.HasValue; }

        //public bool ShouldSerializeLineLimit() { return LineLimit.HasValue; }

        // 对于可空字段，须作如下设置

        [XmlAttribute("dir")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int XmlDirection { get { return (int)Direction.Value; } set { Direction = (RenderDirection)value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool XmlDirectionSpecified { get { return Direction.HasValue; } }

        #endregion

        /// <summary>
        /// 对cell索引表达式进行计算
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public string CalculateCellName(object data)
        {
            var matches = Regex.Matches(CellName, @"\{\+([\w\s+.]+)\}");
            if (matches.Count == 0)
            {
                return CellName;
            }


            var length = CellName.Length;
            int rowGrow = 0;
            int colGrow = 0;

            // 从cell表达式中获取原始cell坐标
            foreach (Match item in matches)
            {
                CellName = CellName.Replace(item.Value, string.Empty);
            }
            var location = ExcelHandler.CellNameToLocation(CellName);


            if (matches.Count == 1)
            {
                var match = matches[0];
                var grow = RazorTemplateHelper.RunRazorSnippet<int>(match.Groups[1].Value, data);
                if (matches[0].Length + matches[0].Index == length)
                {
                    rowGrow = grow;
                }
                else
                {
                    colGrow = grow;
                }
            }
            else
            {
                if (matches.Count == 2)
                {
                    colGrow = RazorTemplateHelper.RunRazorSnippet<int>(matches[0].Groups[1].Value, data);
                    rowGrow = RazorTemplateHelper.RunRazorSnippet<int>(matches[1].Groups[1].Value, data);
                }
            }

            location.RowIndex += rowGrow;
            location.ColumnIndex += colGrow;

            return ExcelHandler.LocationToCellName(location.ColumnIndex, location.RowIndex);
        }
    }

}
