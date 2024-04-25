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
                .HasMaxLength(100);

            // Many-to-Many relationship with Product
            builder.HasMany(pc => pc.Products)
           .WithMany(p => p.Categories)
           .UsingEntity<Dictionary<string, object>>(
               "ProductCategoryMapping", // Define the join table name
               right => right.HasOne<Product>().WithMany().HasForeignKey("ProductId"), 
               left => left.HasOne<ProductCategory>().WithMany().HasForeignKey("CategoryId"),
               joinEntity =>
               {
                   joinEntity.ToTable("ProductCategoryMappings"); 
                   joinEntity.HasKey(new string[] { "ProductId", "CategoryId" });
               });
        }
    }
}
