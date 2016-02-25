using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Tests
{
    public enum TestEnum
    {
        [Display(Name = "测试")]
        [System.ComponentModel.Description("描述")]
        A,

        B
    }
}