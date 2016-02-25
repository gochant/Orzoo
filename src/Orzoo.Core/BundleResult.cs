using System.Collections.Generic;

namespace Orzoo.Core
{
    /// <summary>
    /// 列表批量处理的结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BundleResult<T> where T : class, new()
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// 是否有错
        /// </summary>
        public bool HasError { get; set; } = false;

        /// <summary>
        /// 错误
        /// </summary>
        public List<List<KeyValuePair<string, string>>> Errors { get; set; } = new List<List<KeyValuePair<string, string>>>();
    }
}
