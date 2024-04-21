using Ardalis.Specification;
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MobyLabWebProgramming.Core.Specifications
{
    public sealed class UserProfileByUsernameSpec : BaseSpec<UserProfileByUsernameSpec, UserProfile>
    {
        public UserProfileByUsernameSpec(string username)
        {
            Query
                .Include(up => up.User)  // Ensure the User is included for access to the Username
                .Where(up => up.User.Username == username);
        }
    }
}
