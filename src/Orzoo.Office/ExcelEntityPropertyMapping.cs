using System.Collections.Generic;

namespace Orzoo.Office
{
    public class ExcelEntityPropertyMapping
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 单元格名称
        /// </summary>
        public string CellName { get; set; }

        /// <summary>
        /// 单元格值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 子级数据
        /// </summary>
        public List<ExcelEntityMapping> Items { get; set; }

        public ExcelEntityPropertyMapping(string propName, string cellName)
        {
            PropertyName = propName;
            CellName = cellName;
        }

        public ExcelEntityPropertyMapping(string propName, string cellName, object value) : this(propName, cellName)
        {
            Value = value;
        }
    }

}
