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
        public static void Assert(bool condition, string msg = "逻辑验证失败", AlertType level = AlertType.Warning)
        {
            if (!condition)
            {
                throw new LogicException(msg, level);
            }
        }
    }
}