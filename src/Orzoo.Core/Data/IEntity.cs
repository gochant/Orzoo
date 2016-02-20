using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Data
{
    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    public interface IEntity<TKey>
    {
        [Key]
        [UIHint("Hidden")]
        TKey Id { get; set; }
    }

    /// <summary>
    /// 实体
    /// </summary>
    public interface IEntity : IEntity<string>
    {
    }

}
