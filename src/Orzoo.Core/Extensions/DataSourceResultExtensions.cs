using Orzoo.Core.Linq;

namespace Orzoo.Core.Extensions
{
    public static class DataSourceResultExtensions
    {
        /// <summary>
        /// 生成反馈
        /// </summary>
        /// <param name="result">数据库查询结果</param>
        /// <returns></returns>
        public static Feedback ToFeedback(this DataSourceResult result)
        {
            return Feedback.List(result.Data, result.Total);
        }
    }
}