using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Enums
{
    public enum EditMode
    {
        [Display(Name = "创建")]
        Create,
        [Display(Name = "修改")]
        Modify,
        [Display(Name = "删除")]
        Delete
    }
}