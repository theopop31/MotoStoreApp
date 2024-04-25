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
    public class OrderService : IOrderService
    {
        private readonly IRepository<WebAppDatabaseContext> _repository;

        public OrderService(IRepository<WebAppDatabaseContext> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse> AddOrderAsync(OrderAddDTO orderDTO, UserDTO requestingUser, CancellationToken cancellationToken)
        {
            var userSpec = new UserByUsernameSpec(requestingUser.Username);
            var existingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (existingUser == null)
            {
                return ServiceResponse.FromError(CommonErrors.UserNotFound);
            }
            var order = new Order
            {
                Status = orderDTO.Status,
                OrderDate = orderDTO.OrderDate,
                UserId = existingUser.Id,

            };

            await _repository.AddAsync(order, cancellationToken);
            return ServiceResponse.ForSuccess();

        }

        public async Task<ServiceResponse> UpdateOrderAsync(OrderUpdateDTO orderDTO, UserDTO requestingUser, CancellationToken cancellationToken)
        {
            if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel)
            {
                return ServiceResponse.FromError(CommonErrors.NoPermissions);
            }
            var existingOrder = await _repository.GetAsync(new OrderSpec(orderDTO.id), cancellationToken);
            if (existingOrder == null)
            {
                return ServiceResponse.FromError(CommonErrors.OrderNotFound);
            }
            existingOrder.Status = orderDTO.Status ?? existingOrder.Status;
            await _repository.UpdateAsync(existingOrder, cancellationToken);

            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse> DeleteOrderAsync(Guid orderId, UserDTO requestingUser, CancellationToken cancellationToken)
        {
            if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Role != UserRoleEnum.Personnel)
            {
                return ServiceResponse.FromError(CommonErrors.NoPermissions);
            }

            var existingOrder = await _repository.GetAsync(new OrderSpec(orderId), cancellationToken);
            if (existingOrder == null)
            {
                return ServiceResponse.FromError(CommonErrors.OrderNotFound);
            }

            await _repository.DeleteAsync<Order>(orderId, cancellationToken);
            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse<OrderDTO>> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _repository.GetAsync(new OrderProjectionSpec(orderId), cancellationToken);
            if (order == null)
            {
                return ServiceResponse<OrderDTO>.FromError(CommonErrors.OrderNotFound);
            }

            return ServiceResponse<OrderDTO>.ForSuccess(order);
        }

        public async Task<ServiceResponse<PagedResponse<OrderDTO>>> GetOrdersByUsername(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
        {
            var result = await _repository.PageAsync(pagination, new OrderProjectionSpec(pagination.Search), cancellationToken);

            return ServiceResponse<PagedResponse<OrderDTO>>.ForSuccess(result);
        }

    }
}
