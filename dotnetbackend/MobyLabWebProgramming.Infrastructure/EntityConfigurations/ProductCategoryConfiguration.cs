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
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategories");

            // Primary Key
            builder.Property(pc => pc.Id)
           .IsRequired()
           .ValueGeneratedOnAdd();
            builder.HasKey(pc => pc.Id);

            // Property Configurations
            builder.Property(pc => pc.CategoryName)
                .IsRequired()
                .HasMaxLength(100);  // Adjust the maximum length as necessary for your category names

            // Many-to-Many relationship with Product
            builder.HasMany(pc => pc.Products)
           .WithMany(p => p.Categories)
           .UsingEntity<Dictionary<string, object>>(
               "ProductCategoryMapping", // Define the join table name
               right => right.HasOne<Product>().WithMany().HasForeignKey("ProductId"), // Correctly configure the relationship and foreign key for Product
               left => left.HasOne<ProductCategory>().WithMany().HasForeignKey("CategoryId"), // Correctly configure the relationship and foreign key for ProductCategory
               joinEntity =>
               {
                   joinEntity.ToTable("ProductCategoryMappings"); // Optionally, specify the join table name explicitly again
                   joinEntity.HasKey(new string[] { "ProductId", "CategoryId" }); // Define composite key for the join table
               });
        }
    }
}
