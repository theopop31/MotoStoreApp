namespace MobyLabWebProgramming.Core.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = default!;
        public virtual User User { get; set; } = default!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
    }
}
