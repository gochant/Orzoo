using System;

namespace Orzoo.Core
{
    /// <summary>
    /// 消息等级
    /// </summary>
    [Serializable]
    public enum AlertType
    {
        /// <summary>
        /// 静默
        /// </summary>
        Silent,
        /// <summary>
        /// 消息
        /// </summary>
        Info,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 错误
        /// </summary>
        Error
    }
}