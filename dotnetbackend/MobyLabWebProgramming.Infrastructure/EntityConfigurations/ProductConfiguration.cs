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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            // Primary Key
            builder.Property(p => p.Id)
           .IsRequired()
           .ValueGeneratedOnAdd();
            builder.HasKey(p => p.Id);

            // Property Configurations
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Stock)
                .HasColumnType("decimal(18,2)");

            // Relationships
            // Producer (Many Products to One Producer)
            builder.HasOne(p => p.Producer)
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.ProducerId) // This adds the ProducerId foreign key.
                .OnDelete(DeleteBehavior.Cascade); // Configuring cascade delete if necessary, adjust according to business rules

            // ProductCategory (Many-to-Many with ProductCategory)
            builder.HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategoryMapping", // Table name
                    j => j.HasOne<ProductCategory>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId")
                );

            // OrderDetails (One Product to Many OrderDetails)
            builder.HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId);
        }
    }
}
