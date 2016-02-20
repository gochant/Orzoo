using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orzoo.Core.Data
{
    /// <summary>
    /// 数据变更记录
    /// </summary>
    public class DataChangeLog : Entity, INamedEntity
    {
        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime DateTime { get; set; }

        public virtual ICollection<DataChangeLogItem> DataChangeLogItems { get; set; } = new List<DataChangeLogItem>();
    }

    /// <summary>
    /// 数据变更记录项
    /// </summary>
    public class DataChangeLogItemCore : Entity
    {

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }


        public string DataChangeLogId { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public object CurrentValue { get; set; }

        /// <summary>
        /// 原始值
        /// </summary>
        public object OriginalValue { get; set; }
    }

    public class DataChangeLogItem : DataChangeLogItemCore
    {
        [ForeignKey("DataChangeLogId")]
        public virtual DataChangeLog DataChangeLog { get; set; }
    }
}