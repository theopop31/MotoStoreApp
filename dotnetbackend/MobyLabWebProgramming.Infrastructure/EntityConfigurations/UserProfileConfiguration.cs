using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the UserFile entity.
/// </summary>
public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        // UserProfile uses the same ID as User for a one-to-one relationship
        builder.HasKey(up => up.UserId);

        // Configuring the UserId as both Primary Key and Foreign Key
        builder.Property(up => up.UserId)
            .ValueGeneratedNever();

        // Property Configurations
        builder.Property(up => up.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(up => up.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(up => up.Address)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(up => up.Phone)
            .HasMaxLength(20);

        builder.Property(up => up.BirthDate)
            .HasColumnType("date");

        // Relationship Configuration
        // This sets up the dependent side of a one-to-one relationship to User
        builder.HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(up => up.UserId);

        // This configures the cascade delete behavior to match your business logic needs
        builder.Navigation(up => up.User)
            .IsRequired();

        // Setting cascade delete behavior: deleting a User should delete the associated UserProfile
        builder.HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
