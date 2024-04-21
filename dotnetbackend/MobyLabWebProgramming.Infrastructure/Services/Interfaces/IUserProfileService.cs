using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<ServiceResponse<UserProfileDTO>> GetUserProfile(string username, CancellationToken cancellationToken = default);
        Task<ServiceResponse> AddUserProfileAsync(UserProfileAddDTO userProfileDto, UserDTO requestingUser, CancellationToken cancellationToken = default);
        Task<ServiceResponse> UpdateUserProfileAsync(UserProfileUpdateDTO userProfileDto, string requestingUsername, CancellationToken cancellationToken = default);
        Task<ServiceResponse> DeleteUserProfileAsync(string username, string requestingUsername, CancellationToken cancellationToken = default);
    }
}
