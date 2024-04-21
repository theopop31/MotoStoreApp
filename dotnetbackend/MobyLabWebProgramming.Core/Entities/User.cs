using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Specifications;

namespace MobyLabWebProgramming.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRoleEnum Role { get; set; } = default!;
    public Guid UserProfileId { get; set; } = default!;

    public UserProfile UserProfile { get; set; } = default!;

    // Navigation property to represent the one-to-many relationship with Orders
    public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

    public static implicit operator User(UserByUsernameSpec v)
    {
        throw new NotImplementedException();
    }
}
