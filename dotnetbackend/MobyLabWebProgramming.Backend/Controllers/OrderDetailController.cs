using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IUserService _userService;

        public OrderDetailController(IOrderDetailService orderDetailService, IUserService userService)
        {
            _orderDetailService = orderDetailService;
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

        [Authorize(Roles = "Admin, Personnel, Client")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Add([FromBody] OrderDetailAddDTO orderDetailDto)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");
            var response = await _orderDetailService.AddOrderDetailAsync(orderDetailDto, requestingUser);
            if (response.IsOk)
                return Ok(response);
            else
                switch (response.Error.Status)
                {
                    case HttpStatusCode.NotFound: return NotFound(response);
                    case HttpStatusCode.Unauthorized: return Unauthorized(response);
                    case HttpStatusCode.Conflict: return Conflict(response);
                    default: return BadRequest(response);
                }
        }

        [Authorize(Roles = "Admin, Personnel")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResponse>> Delete(Guid id)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _orderDetailService.DeleteOrderDetailAsync(id, requestingUser);
            if (response.IsOk)
                return Ok(response);
            else
                switch (response.Error.Status)
                {
                    case HttpStatusCode.NotFound: return NotFound(response);
                    case HttpStatusCode.Unauthorized: return Unauthorized(response);
                    case HttpStatusCode.Conflict: return Conflict(response);
                    default: return BadRequest(response);
                }
        }

    }
}
