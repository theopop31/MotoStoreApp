using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
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
        var requestingUsername = User.Identity?.Name;
        if (requestingUsername == null)
            return Unauthorized("Requesting user identity not found.");

        // Assume you have a method to fetch the UserDTO from the username, potentially caching it if needed
        var requestingUser = new UserDTO { Username = requestingUsername, Id = Guid.NewGuid() }; // Example UserDTO creation, fetch actual data as necessary

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
