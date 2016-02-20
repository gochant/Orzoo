using System.Collections.Generic;

namespace Orzoo.Core.Data
{
    public class GroupedData<T>
    {
        public string Key { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
