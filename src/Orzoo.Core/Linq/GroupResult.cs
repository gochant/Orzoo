using System.Linq;
using System.Runtime.Serialization;

namespace Orzoo.Core.Linq
{
    public class GroupResult<TKey, TElement>
    {
        [DataMember(Name = "value")]
        public TKey Value { get; set; }

        [DataMember(Name = "items")]
        public IGrouping<TKey, TElement> Items { get; set; }
    }
}
