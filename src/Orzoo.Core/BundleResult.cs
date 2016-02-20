using System.Collections.Generic;

namespace Orzoo.Core
{
    /// <summary>
    /// 列表批量处理的结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BundleResult<T> where T : class, new()
    {
        public List<T> Data { get; set; } = new List<T>();

        public bool HasError { get; set; } = false;

        public List<List<KeyValuePair<string, string>>> Errors { get; set; } = new List<List<KeyValuePair<string, string>>>();
    }
}
