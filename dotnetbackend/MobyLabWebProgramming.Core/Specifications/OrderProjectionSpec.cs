using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
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
            Username = order.User.Username, 
            OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDTO
            {
                ProductName = detail.Product.Name,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice,
                OrderId = detail.OrderId,
                Id = detail.Id
            }).ToList()
        };

        public OrderProjectionSpec(Guid id) : base(id)
        {
        }

        public OrderProjectionSpec(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new ArgumentException("Search parameter must not be empty.", nameof(search));
            }

            var searchPattern = $"%{search.Trim().Replace(" ", "%")}%";

            Query
                .Include(order => order.User)
                .Where(order => EF.Functions.ILike(order.User.Username, searchPattern));
        }
    }
}
