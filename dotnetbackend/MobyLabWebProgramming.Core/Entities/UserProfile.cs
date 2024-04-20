namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for another entity to store files and an example for a One-To-Many relation.
/// </summary>
public class UserProfile : BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateTime? BirthDate { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
