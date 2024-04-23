using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

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

    [Authorize]
    [HttpGet("{username}")]
    public async Task<ActionResult<ServiceResponse<UserProfileDTO>>> GetByUsername(string username)
    {
        return Ok(await _userProfileService.GetUserProfile(username));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<UserProfileDTO>>> Add([FromBody] UserProfileAddDTO userProfileDto)
    {
        var requestingUser = await GetUserDTO();
        if (requestingUser == null)
            return Unauthorized("Requesting user identity not found.");

        var response = await _userProfileService.AddUserProfileAsync(userProfileDto, requestingUser);

        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<UserProfileDTO>>> Update([FromBody] UserProfileUpdateDTO userProfileDto)
    {
        var requestingUsername = User.Identity?.Name;
        if (requestingUsername == null)
            return Unauthorized("Requesting user identity not found.");

        return Ok(await _userProfileService.UpdateUserProfileAsync(userProfileDto, requestingUsername));
    }

    [Authorize]
    [HttpDelete("{username}")]
    public async Task<ActionResult<ServiceResponse>> Delete(string username)
    {
        var requestingUsername = User.Identity?.Name;
        if (requestingUsername == null)
            return Unauthorized("Requesting user identity not found.");

        return Ok(await _userProfileService.DeleteUserProfileAsync(username, requestingUsername));
    }
}
