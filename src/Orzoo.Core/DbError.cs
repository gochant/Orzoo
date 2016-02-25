namespace Orzoo.Core
{
    /// <summary>
    /// 数据库错误
    /// </summary>
    public class DbError : IDbError
    {
        /// <summary>
        /// 错误数据类型（分组）
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 错误数据字段（编码）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Description { get; set; }
    }
}