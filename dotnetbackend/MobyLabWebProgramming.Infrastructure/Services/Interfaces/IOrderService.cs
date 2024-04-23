using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResponse> AddOrderAsync(OrderAddDTO orderDTO, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse> UpdateOrderAsync(OrderUpdateDTO orderDTO, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse> DeleteOrderAsync(Guid orderId, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse<OrderDTO>> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    }
}
