using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Extensions;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using System.Net;

namespace MobyLabWebProgramming.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProducerController : ControllerBase
    {
        private readonly IProducerService _producerService;
        private readonly IUserService _userService;

        public ProducerController(IProducerService producerService, IUserService userService)
        {
            _producerService = producerService;
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
        public async Task<ActionResult<ServiceResponse<ProducerDTO>>> GetById(Guid id)
        {
            var response = await _producerService.GetProducerByIdAsync(id);

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

        [Authorize(Roles = "Admin, Producer")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Add([FromBody] ProducerAddDTO producerDto)
        {
            var requestingUsername = await GetUserDTO(); 
            if (requestingUsername == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _producerService.AddProducerAsync(producerDto, requestingUsername);

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

        [Authorize(Roles = "Admin, Producer")]
        [HttpPut]
        public async Task<ActionResult<ServiceResponse>> Update([FromBody] ProducerUpdateDTO producerDto)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _producerService.UpdateProducerAsync(producerDto, requestingUser);
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

        [Authorize(Roles = "Admin, Producer")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResponse>> Delete(Guid id)
        {
            var requestingUser = await GetUserDTO();
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _producerService.DeleteProducerAsync(id, requestingUser);
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
