using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;
    private readonly IUserService _userService;

    public UserProfileController(IUserProfileService userProfileService, IUserService userService)
    {
        _userProfileService = userProfileService;
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

    [Authorize(Roles = "Client, Personnel, Producer, Admin")]
    [HttpGet("{username}")]
    public async Task<ActionResult<ServiceResponse<UserProfileDTO>>> GetByUsername(string username)
    {
        var response = await _userProfileService.GetUserProfile(username);
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

    [Authorize(Roles = "Client, Admin")]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<UserProfileDTO>>> Add([FromBody] UserProfileAddDTO userProfileDto)
    {
        var requestingUser = await GetUserDTO();
        if (requestingUser == null)
            return Unauthorized("Requesting user identity not found.");

        var response = await _userProfileService.AddUserProfileAsync(userProfileDto, requestingUser);

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

    [Authorize(Roles = "Client, Admin")]
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<UserProfileDTO>>> Update([FromBody] UserProfileUpdateDTO userProfileDto)
    {
        var requestingUsername = User.Identity?.Name;
        if (requestingUsername == null)
            return Unauthorized("Requesting user identity not found.");

        var response = await _userProfileService.UpdateUserProfileAsync(userProfileDto, requestingUsername);
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

    [Authorize(Roles = "Client, Admin")]
    [HttpDelete("{username}")]
    public async Task<ActionResult<ServiceResponse>> Delete(string username)
    {
        var requestingUsername = User.Identity?.Name;
        if (requestingUsername == null)
            return Unauthorized("Requesting user identity not found.");

        var response = await _userProfileService.DeleteUserProfileAsync(username, requestingUsername);
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
