using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Orzoo.Core.Data;

namespace Orzoo.AspNet.Logging
{
    [DisplayName("用户动态")]
    public class UserActivity : UserActivityCore, IDbEntity
    {
    }

    /// <summary>
    /// 用户活动（动态）
    /// </summary>
    public class UserActivityCore : Entity
    {
        #region Properties, Indexers

        /// <summary>
        /// 操作人IP
        /// </summary>
        [Display(Name = "操作人IP")]
        public string Ip { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [Display(Name = "操作人")]
        public string Operator { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        [Display(Name = "操作日期")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 动作名称
        /// </summary>
        [Display(Name = "动作名称")]
        public string ActionName { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        [Display(Name = "对象ID")]
        public string ObjectId { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        [Display(Name = "对象类型")]
        public string ObjectType { get; set; }

        /// <summary>
        /// 对象名称
        /// </summary>
        [Display(Name = "对象名称")]
        public string Name { get; set; }

        #endregion
    }
}