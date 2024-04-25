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
    public class UserProfileService : IUserProfileService
    {
        private readonly IRepository<WebAppDatabaseContext> _repository;

        public UserProfileService(IRepository<WebAppDatabaseContext> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<UserProfileDTO>> GetUserProfile(string username, CancellationToken cancellationToken = default)
        {
            var spec = new UserProfileByUsernameSpec(username);
            var userProfile = await _repository.GetAsync(spec, cancellationToken);

            if (userProfile == null)
            {
                return ServiceResponse<UserProfileDTO>.FromError(new (HttpStatusCode.NotFound, "UserProfile not found."));
            }

            return ServiceResponse<UserProfileDTO>.ForSuccess(MapToUserProfileDTO(userProfile));
        }

        public async Task<ServiceResponse> AddUserProfileAsync(UserProfileAddDTO userProfileDto, UserDTO requestingUser, CancellationToken cancellationToken = default)
        {
            var userSpec = new UserByUsernameSpec(requestingUser.Username);
            var existingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (existingUser == null)
            {
                return ServiceResponse.FromError(new (HttpStatusCode.NotFound, "Requesting user does not exist."));
            }

            var userProfileSpec = new UserProfileByUsernameSpec(requestingUser.Username);
            var existingUserProfile = await _repository.GetAsync(userProfileSpec, cancellationToken);
            if (existingUserProfile != null)
            {
                return ServiceResponse.FromError(new (HttpStatusCode.Forbidden, "UserProfile already exists."));
            }

            var userProfile = new UserProfile
            {
                FirstName = userProfileDto.FirstName,
                LastName = userProfileDto.LastName,
                Address = userProfileDto.Address,
                Phone = userProfileDto.Phone,
                BirthDate = userProfileDto.BirthDate,
                UserId = existingUser.Id,
            };

            await _repository.AddAsync(userProfile, cancellationToken);
            return ServiceResponse.ForSuccess();
        }


        public async Task<ServiceResponse> UpdateUserProfileAsync(UserProfileUpdateDTO userProfileDto, string requestingUsername, CancellationToken cancellationToken = default)
        {
            var userSpec = new UserByUsernameSpec(requestingUsername);
            var requestingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (requestingUser == null)
            {
                return ServiceResponse.FromError(CommonErrors.UserNotFound);
            }

            if (requestingUsername != userProfileDto.Username || requestingUser.Role != UserRoleEnum.Admin)
            {
                return ServiceResponse.FromError(CommonErrors.NoPermissions);
            }

            var spec = new UserProfileByUsernameSpec(userProfileDto.Username);
            var userProfile = await _repository.GetAsync(spec, cancellationToken);
            if (userProfile == null)
            {
                return ServiceResponse.FromError(CommonErrors.UserProfileNotFound);
            }

            userProfile.FirstName = userProfileDto.FirstName ?? userProfile.FirstName;
            userProfile.LastName = userProfileDto.LastName ?? userProfile.LastName;
            userProfile.Address = userProfileDto.Address ?? userProfile.Address;
            userProfile.Phone = userProfileDto.Phone ?? userProfile.Phone;
            userProfile.BirthDate = userProfileDto.BirthDate ?? userProfile.BirthDate;

            await _repository.UpdateAsync(userProfile, cancellationToken);
            return ServiceResponse.ForSuccess();
        }

        public async Task<ServiceResponse> DeleteUserProfileAsync(string username, string requestingUsername, CancellationToken cancellationToken = default)
        {
            var userSpec = new UserByUsernameSpec(requestingUsername);
            var requestingUser = await _repository.GetAsync(userSpec, cancellationToken);
            if (requestingUser == null)
            {
                return ServiceResponse.FromError(CommonErrors.UserNotFound);
            }
            Console.WriteLine(requestingUsername);
            if (requestingUsername != username)
            {
                return ServiceResponse.FromError(CommonErrors.NoPermissions);
            }

            var spec = new UserProfileByUsernameSpec(username);
            var userProfile = await _repository.GetAsync(spec, cancellationToken);
            if (userProfile == null)
            {
                return ServiceResponse.FromError(CommonErrors.UserProfileNotFound);
            }
            Console.WriteLine(userProfile.UserId);
            await _repository.DeleteAsyncUserFile<UserProfile>(userProfile.UserId, cancellationToken);
            return ServiceResponse.ForSuccess();
        }

        private UserProfileDTO MapToUserProfileDTO(UserProfile userProfile)
        {
            return new UserProfileDTO
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Address = userProfile.Address,
                Phone = userProfile.Phone,
                BirthDate = userProfile.BirthDate,
                Username = userProfile.User.Username
            };
        }
    }

}
