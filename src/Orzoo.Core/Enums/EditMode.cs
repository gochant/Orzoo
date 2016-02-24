using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Enums
{
    /// <summary>
    /// 编辑模式
    /// </summary>
    public enum EditMode
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Display(Name = "创建")]
        Create,
        /// <summary>
        /// 修改
        /// </summary>
        [Display(Name = "修改")]
        Modify,
        /// <summary>
        /// 删除
        /// </summary>
        [Display(Name = "删除")]
        Delete
    }
}