using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces
{
    public interface IProducerService
    {
        Task<ServiceResponse> AddProducerAsync(ProducerAddDTO producerDto, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse> UpdateProducerAsync(ProducerUpdateDTO producerDto, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse> DeleteProducerAsync(Guid producerId, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse<ProducerDTO>> GetProducerByIdAsync(Guid producerId, CancellationToken cancellationToken = default);
    }
}
