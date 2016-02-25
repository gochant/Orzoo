using System;
using System.Runtime.Serialization;

namespace Orzoo.Core
{
    /// <summary>
    /// 业务逻辑异常
    /// </summary>
    [Serializable]
    public class LogicException : Exception
    {
        /// <summary>
        /// 异常等级
        /// </summary>
        public virtual AlertType Level { get; set; }

        /// <summary>
        /// 异常的键
        /// </summary>
        public string Key { get; set; }

        public LogicException() : base() { }

        public LogicException(string message) : base(message)
        {
            Level = AlertType.Error;

        }

        public LogicException(string message, AlertType level) : base(message)
        {
            Level = level;
        }

        public LogicException(string format, params object[] args) : base(string.Format(format, args)) { }

        public LogicException(string message, Exception innerException) : base(message, innerException) { }

        public LogicException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        protected LogicException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
