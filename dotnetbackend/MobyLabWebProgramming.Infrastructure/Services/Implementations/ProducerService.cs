using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
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
    public class ProducerService : IProducerService
    {
        private readonly IRepository<WebAppDatabaseContext> _repository;

        public ProducerService(IRepository<WebAppDatabaseContext> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse> AddProducerAsync(ProducerAddDTO producerDto, UserDTO requestingUser, CancellationToken cancellationToken = default)
        {
            if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Producer)
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "The user does not have admin or producer permissions!", ErrorCodes.CannotAdd));
            }

            var result = await _repository.GetAsync(new ProducerSpec(producerDto.ProducerName), cancellationToken);

            if (result != null)
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The producer already exists!", ErrorCodes.ProducerAlreadyExists));
            }
            
            var producer = new Producer
            {
                ProducerName = producerDto.ProducerName,
                ContactInfo = producerDto.ContactInfo
            };

            await _repository.AddAsync(producer, cancellationToken);
            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse> UpdateProducerAsync(ProducerUpdateDTO producerDto, UserDTO requestingUser, CancellationToken cancellationToken = default)
        {
            if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Producer)
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "The user does not have admin or producer permissions!", ErrorCodes.NotEnoughPermissions));
            }

            var result = await _repository.GetAsync(new ProducerSpec(producerDto.Id), cancellationToken);

            if (result == null)
            {
                return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "The producer doesn't exist!", ErrorCodes.EntityNotFound));
            }

            result.ProducerName = producerDto.ProducerName ?? result.ProducerName;
            result.ContactInfo = producerDto.ContactInfo ?? result.ContactInfo;
            await _repository.UpdateAsync(result, cancellationToken);

            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse> DeleteProducerAsync(Guid producerId, UserDTO requestingUser, CancellationToken cancellationToken = default)
        {
            if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Producer)
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "The user does not have admin permissions!", ErrorCodes.NotEnoughPermissions));
            }

            await _repository.DeleteAsync<Producer>(producerId, cancellationToken);
          
            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse<ProducerDTO>> GetProducerByIdAsync(Guid producerId, CancellationToken cancellationToken = default)
        {
            var producer = await _repository.GetAsync(new ProducerProjectionSpec(producerId), cancellationToken);
            if (producer == null)
            {
                return ServiceResponse<ProducerDTO>.FromError(new(HttpStatusCode.NotFound, "The producer doesn't exist!", ErrorCodes.EntityNotFound));
            }

            return ServiceResponse<ProducerDTO>.ForSuccess(producer);
          
        }
    }
}
