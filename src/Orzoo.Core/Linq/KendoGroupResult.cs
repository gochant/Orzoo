using System.Linq;

namespace Orzoo.Core.Linq
{
    public class KendoGroupResult<TKey, TElement>
    {
        public TKey value { get; set; }

        public IGrouping<TKey, TElement> items { get; set; }
    }
}
