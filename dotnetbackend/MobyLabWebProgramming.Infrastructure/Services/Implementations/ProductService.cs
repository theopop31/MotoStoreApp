using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IRepository<WebAppDatabaseContext> _repository;

        public ProductService(IRepository<WebAppDatabaseContext> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse> AddProductAsync(ProductAddDTO productDto, UserDTO requestingUser, CancellationToken cancellationToken = default)
        {
            var userSpec = new UserByUsernameSpec(requestingUser.Username);
            var existingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (existingUser != null && existingUser.Role != UserRoleEnum.Admin && existingUser.Role != UserRoleEnum.Producer)
            {
                return ServiceResponse.FromError(CommonErrors.NoPermissions);
            }

            var producerSpec = new ProducerSpec(productDto.ProducerName);
            var producer = await _repository.GetAsync(producerSpec, cancellationToken);
            if (producer == null)
            {
                return ServiceResponse.FromError(CommonErrors.ProducerNotFound);
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                ProducerId = producer.Id,
                // Categories = (ICollection<ProductCategory>)productDto.Categories
            };

            await _repository.AddAsync(product, cancellationToken);
            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse> UpdateProductAsync(ProductUpdateDTO productDto, UserDTO requestingUser, CancellationToken cancellationToken = default)
        {
            var userSpec = new UserByUsernameSpec(requestingUser.Username);
            var existingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (existingUser != null && existingUser.Role != UserRoleEnum.Admin && existingUser.Role != UserRoleEnum.Producer)
            {
                return ServiceResponse.FromError(CommonErrors.NoPermissions);
            }

            var product = await _repository.GetAsync(new ProductSpec(productDto.id), cancellationToken);
            if (product == null)
            {
                return ServiceResponse.FromError(CommonErrors.ProductNotFound);
            }

            product.Name = productDto.Name ?? product.Name;
            product.Description = productDto.Description ?? product.Description;
            product.Price = productDto.Price ?? product.Price;
            product.Stock = productDto.Stock ?? product.Stock;

            if (productDto.ProducerName != null)
            {
                var producerSpec = new ProducerSpec(productDto.ProducerName);
                var producer = await _repository.GetAsync(producerSpec, cancellationToken);
                if (producer != null)
                {
                    product.ProducerId = producer.Id;
                }
            }

            /* if (productDto.Categories != null)
             {

                 product.Categories = (ICollection<ProductCategory>)productDto.Categories;
             }*/

            await _repository.UpdateAsync(product, cancellationToken);
            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse> DeleteProductAsync(Guid productId, UserDTO requestingUser, CancellationToken cancellationToken = default)
        {
            var userSpec = new UserByUsernameSpec(requestingUser.Username);
            var existingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (existingUser != null && existingUser.Role != UserRoleEnum.Admin && existingUser.Role != UserRoleEnum.Producer)
            {
                return ServiceResponse.FromError(CommonErrors.NoPermissions);
            }
            await _repository.DeleteAsync<Product>(productId, cancellationToken);
            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetAsync(new ProductProjectionSpec(productId), cancellationToken);
            if (product == null)
            {
                return ServiceResponse<ProductDTO>.FromError(CommonErrors.ProductNotFound);
            }

            return ServiceResponse<ProductDTO>.ForSuccess(product);
        }

        public async Task<ServiceResponse<PagedResponse<ProductDTO>>> GetProductsByProducer(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
        {
            var result = await _repository.PageAsync(pagination, new ProductProjectionSpec(pagination.Search), cancellationToken);
            return ServiceResponse<PagedResponse<ProductDTO>>.ForSuccess(result);
        }
    }
}
