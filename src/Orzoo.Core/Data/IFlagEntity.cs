using Orzoo.Core.Enums;

namespace Orzoo.Core.Data
{
    /// <summary>
    /// 带数据标记的实体
    /// </summary>
    public interface IFlagEntity
    {
        /// <summary>
        /// 数据标记（是否有效）
        /// </summary>
        DataFlag? Flag { get; set; }
    }
}
