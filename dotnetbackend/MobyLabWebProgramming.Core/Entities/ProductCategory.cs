namespace MobyLabWebProgramming.Core.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string CategoryName { get; set; } = default!;

        // Direct many-to-many relationship to Product
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
