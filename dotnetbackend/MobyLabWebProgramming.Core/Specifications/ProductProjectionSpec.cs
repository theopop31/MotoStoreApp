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
    public sealed class ProductProjectionSpec : BaseSpec<ProductProjectionSpec, Product, ProductDTO>
    {
        protected override Expression<Func<Product, ProductDTO>> Spec => product => new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = (decimal)product.Price,
            Stock = (decimal)product.Stock,
            ProducerName = product.Producer.ProducerName,
            /*Categories = product.Categories.Select(pc => new ProductCategoryDTO
            {
                Id = pc.Id,
                CategoryName = pc.CategoryName
            }).ToList(),*/
            OrderDetails = product.OrderDetails.Select(od => new OrderDetailDTO
            {
                ProductName = od.Product.Name,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            }).ToList()
        };

        public ProductProjectionSpec(Guid id) : base(id)
        {
            /*Query
                .Include(p => p.Producer)
                .Include(p => p.Categories)
                .Include(p => p.OrderDetails).ThenInclude(od => od.Order);*/ // Include necessary navigation properties
        }

        public ProductProjectionSpec(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new ArgumentException("Search parameter must not be empty.", nameof(search));
            }

            var searchPattern = $"%{search.Trim().Replace(" ", "%")}%";

            Query
                .Include(p => p.Producer)
                //.Include(p => p.Categories)
                .Where(p => EF.Functions.ILike(p.Name, searchPattern) || EF.Functions.ILike(p.Producer.ProducerName, searchPattern));
        }
    }

}
