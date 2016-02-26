namespace Orzoo.Core
{
    /// <summary>
    /// 业务逻辑契约
    /// </summary>
    public class LogicContract
    {
        /// <summary>
        /// 断言
        /// </summary>
        /// <param name="condition">成功条件</param>
        /// <param name="msg">失败消息</param>
        /// <param name="level">消息等级</param>
        public static void Assert(bool condition, string msg = null, AlertType level = AlertType.Warning)
        {
            msg = msg ?? Properties.Resources.LogicVerificationFailed;
            if (!condition)
            {
                throw new LogicException(msg, level);
            }
        }
    }
}