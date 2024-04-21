using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Specifications
{
    public sealed class UserProfileSpec : BaseSpec<UserProfileSpec, UserProfile>
    {
        // Constructor for fetching a single user profile by UserId
        public UserProfileSpec(Guid userId) : base(userId)
        {
        }

        // Constructor for fetching multiple user profiles by a list of UserIds
        public UserProfileSpec(ICollection<Guid> userIds, bool orderByCreatedAt = true)
            : base(userIds, orderByCreatedAt)
        {
        }
    }

}
