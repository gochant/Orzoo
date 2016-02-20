using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Data
{
    public class SimpleDto : ISimpleDto
    {
        [UIHint("Hidden")]
        public string Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }
    }
}
