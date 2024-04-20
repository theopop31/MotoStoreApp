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
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {

            // Primary Key
            builder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.ProducerName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.ContactInfo)
                .HasMaxLength(500);

            // Relationships
            // One-to-Many relationship with Product
            builder.HasMany(p => p.Products)
                .WithOne(prod => prod.Producer)
                .HasForeignKey(prod => prod.ProducerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
