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

        [Authorize]  // Assuming roles are "Admin" and "Producer"
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResponse<ProducerDTO>>> GetById(Guid id)
        {
            var response = await _producerService.GetProducerByIdAsync(id);

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Add([FromBody] ProducerAddDTO producerDto)
        {
            var requestingUsername = await GetUserDTO();  // Implement this method based on your authorization logic
            if (requestingUsername == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _producerService.AddProducerAsync(producerDto, requestingUsername);

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ServiceResponse>> Update([FromBody] ProducerUpdateDTO producerDto)
        {
            var requestingUser = await GetUserDTO();  // Implement this method based on your authorization logic
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _producerService.UpdateProducerAsync(producerDto, requestingUser);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResponse>> Delete(Guid id)
        {
            var requestingUser = await GetUserDTO();  // Implement this method based on your authorization logic
            if (requestingUser == null)
                return Unauthorized("User must be logged in to perform this action.");

            var response = await _producerService.DeleteProducerAsync(id, requestingUser);
            return Ok(response);
        }

    }
}
