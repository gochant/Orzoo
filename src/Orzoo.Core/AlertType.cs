using System;

namespace Orzoo.Core
{
    /// <summary>
    /// 消息等级
    /// </summary>
    [Serializable]
    public enum AlertType
    {
        Silent,
        Info,
        Success,
        Warning,
        Error
    }
}