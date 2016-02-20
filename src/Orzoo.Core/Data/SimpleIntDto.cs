namespace Orzoo.Core.Data
{
    public class SimpleIntDto : INamedEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}