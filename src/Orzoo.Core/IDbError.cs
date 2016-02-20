namespace Orzoo.Core
{
    /// <summary>
    /// 数据库端的错误
    /// </summary>
    public interface IDbError
    {
        string Code { get; set; }

        string Description { get; set; }
    }
}