using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications
{
    public sealed class ProducerProjectionSpec : BaseSpec<ProducerProjectionSpec, Producer, ProducerDTO>
    {
        protected override Expression<Func<Producer, ProducerDTO>> Spec => e => new()
        {
            Id = e.Id,
            ProducerName = e.ProducerName,
            ContactInfo = e.ContactInfo,
            Products = e.Products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = (decimal)product.Price,
                Stock = (decimal)product.Stock,
                ProducerName = product.Name,
                /*Categories = product.Categories.Select(category => new ProductCategoryDTO
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                    // Additional category mappings if necessary
                }).ToList()*/
            }).ToList()
        };

        public ProducerProjectionSpec(Guid id) : base(id)
        {
        }
    }
}
