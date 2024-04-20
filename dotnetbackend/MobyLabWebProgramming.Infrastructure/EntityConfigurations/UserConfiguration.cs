using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the User entity, generally the Entity Framework will figure out most of the configuration but,
/// for some specifics such as unique keys, indexes and foreign keys it is better to explicitly specify them.
/// Note that the EntityTypeBuilder implements a Fluent interface, meaning it is a highly declarative interface using method-chaining.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd(); // We want the id to be handled by the database automatically 
        builder.HasKey(x => x.Id); // Id is PK
        builder.Property(e => e.Username)
            .HasMaxLength(255) 
            .IsRequired();
        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.Role)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Unique Constraints
        builder.HasIndex(e => e.Username).IsUnique();
        builder.HasIndex(e => e.Email).IsUnique();

        // Relationships
        // One-to-One relationship with the UserProfile table
        builder.HasOne(u => u.UserProfile)
            .WithOne(p => p.User)
            .HasForeignKey<User>(u => u.UserProfileId);

    }
}
