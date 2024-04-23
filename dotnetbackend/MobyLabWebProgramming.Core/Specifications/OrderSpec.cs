using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Specifications
{
    public sealed class OrderSpec : BaseSpec<OrderSpec, Order>
    {
        public OrderSpec(Guid id) : base(id)
        {
            Query.Where(p => p.Id == id);
        }
    }

}
