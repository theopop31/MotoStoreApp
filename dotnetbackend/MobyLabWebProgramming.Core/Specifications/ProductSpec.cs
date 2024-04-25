using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Specifications
{
    public sealed class ProductSpec : BaseSpec<ProductSpec, Product>
    {
        public ProductSpec(Guid id) : base(id)
        {
            Query.Where(p => p.Id == id);
        }

        public ProductSpec(string ProductName)
        {
            Query.Where(e => e.Name == ProductName);
        }
    }
}
