using System.Collections.Concurrent;

namespace MobyLabWebProgramming.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal ? Price { get; set; }
        public decimal? Stock { get; set;}
        public Guid? ProducerId { get; set; } // Foreign Key
        public Producer Producer { get; set; } = default!;

        // Direct many-to-many relationship to ProductCategory
        public virtual ICollection<ProductCategory> Categories { get; set; } = new HashSet<ProductCategory>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
    }
}
