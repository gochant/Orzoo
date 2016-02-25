namespace Orzoo.Core.Data
{
    /// <summary>
    /// 访问安全控制的实体
    /// </summary>
    public interface IAccessSecurityEntity :  IEntity, IMetadataEntity
    {
        ///// <summary>
        ///// 用户定义的可访问者
        ///// </summary>
        string Accesses { get; set; }

        /// <summary>
        /// 获取数据安全性的根
        /// </summary>
        /// <returns></returns>
        string GetAccessSecurityRoot();

        /// <summary>
        /// 是否启用可访问权限
        /// </summary>
        /// <returns></returns>
        bool EnableAccessSecurity();
    }
}
