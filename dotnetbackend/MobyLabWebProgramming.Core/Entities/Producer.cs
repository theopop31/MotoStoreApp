namespace MobyLabWebProgramming.Core.Entities
{
    public class Producer : BaseEntity
    {
        public string ProducerName { get; set; } = default!;
        public string ContactInfo { get; set; } = default!;
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
