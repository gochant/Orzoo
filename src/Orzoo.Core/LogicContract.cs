namespace Orzoo.Core
{
    public class LogicContract
    {
        public static void Assert(bool condition, string msg = "逻辑验证失败", AlertType level = AlertType.Warning)
        {
            if (!condition)
            {
                throw new LogicException(msg, level);
            }
        }
    }
}