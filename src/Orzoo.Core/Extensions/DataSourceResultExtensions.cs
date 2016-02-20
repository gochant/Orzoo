using Orzoo.Core.Linq;

namespace Orzoo.Core.Extensions
{
    public static class DataSourceResultExtensions
    {
        public static Feedback ToFeedback(this DataSourceResult result)
        {
            return Feedback.List(result.Data, result.Total);
        }
    }
}