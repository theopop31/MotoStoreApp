using MailKit.Search;
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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IRepository<WebAppDatabaseContext> _repository;

        public OrderDetailService(IRepository<WebAppDatabaseContext> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse> AddOrderDetailAsync(OrderDetailAddDTO orderDetailDTO, UserDTO requestingUser, CancellationToken cancellationToken)
        {
            var userSpec = new UserByUsernameSpec(requestingUser.Username);
            var existingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (existingUser == null)
            {
                return ServiceResponse.FromError(CommonErrors.UserNotFound);
            }

            if (existingUser.Role != UserRoleEnum.Client && existingUser.Role != UserRoleEnum.Admin)
            {
                return ServiceResponse.FromError(CommonErrors.NotAClient);
            }

            var orderSpec = new OrderSpec(orderDetailDTO.OrderId);
            var existingOrder = await _repository.GetAsync(orderSpec, cancellationToken);
            if (existingOrder == null)
            {
                return ServiceResponse.FromError(CommonErrors.OrderNotFound);
            }

            List<Guid> existingOrderIds = existingUser.Orders.Select(order => order.Id).ToList();

            if (existingUser.Role == UserRoleEnum.Client && !existingOrderIds.Contains(orderDetailDTO.OrderId))
            {
                return ServiceResponse.FromError(CommonErrors.BadOrderId);
            }

            var productSpec = new ProductSpec(orderDetailDTO.ProductId);
            var existingProduct = await _repository.GetAsync(productSpec, cancellationToken);
            if (existingProduct == null)
            {
                return ServiceResponse.FromError(CommonErrors.ProductNotFound);
            }

            if (existingProduct.Stock - orderDetailDTO.Quantity < 0)
            {
                return ServiceResponse.FromError(CommonErrors.NoStock);
            }

            var orderDetail = new OrderDetail
            {
                OrderId = orderDetailDTO.OrderId,
                ProductId = orderDetailDTO.ProductId,
                Quantity = orderDetailDTO.Quantity,
                UnitPrice = (decimal)existingProduct.Price

            };
            await _repository.AddAsync(orderDetail, cancellationToken);
            existingProduct.Stock -= orderDetailDTO.Quantity;
            await _repository.UpdateAsync(existingProduct, cancellationToken);
            return ServiceResponse.ForSuccess();

        }

        public async Task<ServiceResponse> DeleteOrderDetailAsync(Guid orderDetailId, UserDTO requestingUser, CancellationToken cancellationToken)
        {
            var userSpec = new UserByUsernameSpec(requestingUser.Username);
            var existingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (existingUser.Role != UserRoleEnum.Client && existingUser.Role != UserRoleEnum.Admin && existingUser.Role != UserRoleEnum.Personnel)
            {
                return ServiceResponse.FromError(CommonErrors.NotAClient);
            }

            var orderDetailSpec = new OrderDetailSpec(orderDetailId);
            var existingOrderDetail = await _repository.GetAsync(orderDetailSpec, cancellationToken);
            if (existingOrderDetail == null)
            {
                return ServiceResponse.FromError(CommonErrors.OrderDetailNotFound);
            }

            List<Guid> existingOrderIds = existingUser.Orders.Select(order => order.Id).ToList();

            if (existingUser.Role == UserRoleEnum.Client && !existingOrderIds.Contains(existingOrderDetail.OrderId))
            {
                return ServiceResponse.FromError(CommonErrors.BadOrderId);
            }

            await _repository.DeleteAsync<OrderDetail>(orderDetailId, cancellationToken);
            return ServiceResponse.ForSuccess();

        }
    }
}
 