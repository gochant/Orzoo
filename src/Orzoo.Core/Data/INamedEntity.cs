namespace Orzoo.Core.Data
{
    /// <summary>
    /// 命名实体
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    public interface INamedEntity<Tkey> : IEntity<Tkey>
    {
        string Name { get; set; }
    }

    /// <summary>
    /// 命名实体
    /// </summary>
    public interface INamedEntity : IEntity, INamedEntity<string>
    {

    }
}
