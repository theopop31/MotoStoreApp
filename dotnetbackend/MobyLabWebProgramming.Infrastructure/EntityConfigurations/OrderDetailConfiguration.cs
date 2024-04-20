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
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {

            // Primary Key
            builder.Property(o => o.Id)
           .IsRequired()
           .ValueGeneratedOnAdd();
            builder.HasKey(od => od.Id);

            // Properties
            builder.Property(od => od.OrderId).IsRequired();
            builder.Property(od => od.ProductId).IsRequired();
            builder.Property(od => od.Quantity).IsRequired();
            builder.Property(od => od.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18, 2)"); 

            // Relationships
            // OrderDetail to Order (Many-to-One)
            builder.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);  // Deleting an Order deletes its OrderDetails

            // OrderDetail to Product (Many-to-One)
            builder.HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of a Product if it is referenced by any OrderDetail
        }
    }
}
