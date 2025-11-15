using DomainLayer.Models.ProductModule;
using Microsoft.EntityFrameworkCore;
using presentation.Data.Configurations;
using Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace presentation.Data.Contexts
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Reference).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
        }
    }
}
