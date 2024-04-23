using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Specifications
{
    public sealed class OrderProjectionSpec : BaseSpec<OrderProjectionSpec, Order, OrderDTO>
    {
        protected override Expression<Func<Order, OrderDTO>> Spec => order => new OrderDTO
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            Username = order.User.Username, // Assuming the User navigation property is correctly configured
            OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDTO
            {
                ProductId = detail.Id,
                ProductName = detail.Product.Name, // Assuming the Product navigation property is available
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice
                // Map other necessary properties
            }).ToList()
        };

        public OrderProjectionSpec(Guid id) : base(id)
        {
        }

        // Add additional constructors for different filtering needs
    }
}
