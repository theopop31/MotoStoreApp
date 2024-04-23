using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        private async Task<UserDTO?> GetUserDTO()
        {
            var username = User.Identity?.Name;
            if (username == null)
            {
                return null;
            }

            return await _userService.GetUserByUsernameAsync(username);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResponse<OrderDTO>>> GetById(Guid id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Add([FromBody] OrderAddDTO orderDto)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _orderService.AddOrderAsync(orderDto, requestingUser);
            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ServiceResponse>> Update([FromBody] OrderUpdateDTO orderDto)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _orderService.UpdateOrderAsync(orderDto, requestingUser);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResponse>> Delete(Guid id)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _orderService.DeleteOrderAsync(id, requestingUser);
            return Ok(response);
        }
    }
}
