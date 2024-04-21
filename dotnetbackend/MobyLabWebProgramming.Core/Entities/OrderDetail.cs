namespace MobyLabWebProgramming.Core.Entities
{
    public class OrderDetail : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Order Order { get; set; } = default!;
        public virtual Product Product { get; set; } = default!;
    }
}
