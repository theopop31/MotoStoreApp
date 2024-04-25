using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Specifications
{
    public sealed class OrderDetailSpec : BaseSpec<OrderDetailSpec, OrderDetail>
    {
        public OrderDetailSpec(Guid id) : base(id)
        {
        }
    }
}
