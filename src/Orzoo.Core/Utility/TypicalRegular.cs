namespace Orzoo.Core.Utility
{
    /// <summary>
    /// 典型的正则表达式
    /// </summary>
    public class TypicalRegular
    {
        /// <summary>
        /// 电话号码（支持手机号码，3-4位区号，7-8位直播号码，1－4位分机号）
        /// </summary>
        public const string PhoneNumber = @"(^(\d{11})$|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";

        /// <summary>
        /// 身份证
        /// </summary>
        public const string CreditCard = @"^\d{15}$|^\d{18}$|^\d{17}(\d|X|x)$";
    }
}
