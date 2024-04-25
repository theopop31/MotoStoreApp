using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse> AddProductAsync(ProductAddDTO productDto, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse> UpdateProductAsync(ProductUpdateDTO productDto, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse> DeleteProductAsync(Guid productId, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<ServiceResponse<PagedResponse<ProductDTO>>> GetProductsByProducer(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    }
}
