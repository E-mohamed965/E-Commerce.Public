using DomainLayer.Models.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace presentation.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.ProductBrand)
                   .WithMany()
                   .HasForeignKey(p => p.BrandId);

            builder.HasOne(p => p.ProductType)
                   .WithMany()
                   .HasForeignKey(p => p.TypeId);
            builder.Property(p=>p.Price)
                   .HasColumnType("decimal(10,2)");
        }
    }
}
