using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task<ServiceResponse> AddOrderDetailAsync(OrderDetailAddDTO orderDetailDTO, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse> DeleteOrderDetailAsync(Guid orderDetailId, UserDTO requestingUser, CancellationToken cancellationToken = default);
    }
}
