using System.Collections.Generic;
using System.Linq;

namespace Orzoo.Core.Linq
{
    /// <summary>
    /// 用于 Kendo UI 的 DataSource 参数
    /// </summary>
    public class AdvanceDataSourceRequest : DataSourceRequest
    {
        public AdvanceDataSourceRequest() : base()
        {
            Take = int.MaxValue;
            Skip = 0;
            Filter = new Filter();
        }

        public List<string> Fields { get; set; } = new List<string>();

        public new List<Sort> Sort { get; set; } = new List<Linq.Sort>();

        public IEnumerable<Aggregator> Aggregates { get; set; } = new List<Aggregator>();

        /// <summary>
        /// 添加筛选器
        /// </summary>
        /// <param name="filter">筛选器对象</param>
        /// <param name="logic">逻辑</param>
        public void AddFilter(Filter filter, string logic = "and")
        {
            Filter.Filters = Filter.Filters ?? new List<Filter>();
            Filter.Filters.Add(filter);
            Filter.Logic = Filter.Logic ?? logic;
        }

        public static AdvanceDataSourceRequest Create(List<Sort> sort = null,
            Filter filter = null,
            int take = int.MaxValue,
            int skip = 0,
            IEnumerable<Aggregator> aggregates = null)
        {
            if (sort == null)
            {
                sort = new List<Sort>();
            }

            if (aggregates == null)
            {
                aggregates = new List<Aggregator>();
            }

            // BugFix: 通过IE8 提交并不带filter后， 后台filter 的 Logic 会被设置为 'and'，暂不知晓原因
            if (filter != null && (filter.Filters != null && !filter.Filters.Any() || filter.Filters == null))
            {
                filter.Logic = null;
            }

            if (filter != null && filter.Field != null && filter.Value != null)
            {
                filter.Logic = "and";
            }

            return new AdvanceDataSourceRequest
            {
                Filter = filter,
                Sort = sort,
                Take = take,
                Skip = skip,
                Aggregates = aggregates
            };
        }

        public static AdvanceDataSourceRequest Create(AdvanceDataSourceRequest request)
        {
            return Create(request.Sort, request.Filter, request.Take, request.Skip, request.Aggregates);
        }

    }
}
