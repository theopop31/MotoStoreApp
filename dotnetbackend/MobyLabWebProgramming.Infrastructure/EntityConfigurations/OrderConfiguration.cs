using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            // Primary Key
            builder.Property(o => o.Id)
           .IsRequired()
           .ValueGeneratedOnAdd();
            builder.HasKey(o => o.Id);

            // Property Configurations
            builder.Property(o => o.UserId).IsRequired();

            builder.Property(o => o.OrderDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(50);

            // Relationships
            // User to Orders (One-to-Many)
            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Order to OrderDetails (One-to-Many)
            builder.HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
