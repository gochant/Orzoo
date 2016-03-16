using System.Collections.Generic;
using System.Linq;

namespace Orzoo.Core.Linq
{
    public class DynamicLinqFilterParameter
    {
        public List<string> Predicates { get; set; } = new List<string>();

        public List<KeyValuePair<string, object>> Args { get; set; } = new List<KeyValuePair<string, object>>();

        public string Logic { get; set; } = "";

        public string GetCombinedPredicate()
        {
            var predicate = string.Join($" {Logic} ", Predicates);
            for (int i = 0; i < Args.Count; i++)
            {
                var arg = Args[i];
                predicate = predicate.Replace($"[{arg.Key}]", $"@{i}");
            }

            return predicate;
        }

        public object[] GetCombinedArgs()
        {
            return Args.Select(d => d.Value).ToArray();
        }
    }
}