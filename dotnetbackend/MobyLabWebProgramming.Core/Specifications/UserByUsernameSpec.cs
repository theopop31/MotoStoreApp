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
    public sealed class UserByUsernameSpec : BaseSpec<UserByUsernameSpec, User>
    {
        public UserByUsernameSpec(string username)
        {
            Query
                .Where(u => u.Username == username);
        }
    }
}
