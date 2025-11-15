using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using presentation.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence
{
    public class DataSeeding(StoreDbContext _dbContext,
                             UserManager<ApplicationUser> _userManager,
                             RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {



        public async Task DataSeedAsync()
        {
            try
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }
                if (!_dbContext.ProductBrands.Any())
                {
                    // var productBrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\DataSeed\brands.json");
                    var productBrandsData = File.OpenRead(@"..\Infrastructure\Presistence\Data\DataSeed\brands.json");
                    var productBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandsData);
                    if (productBrands is not null && productBrands.Any())
                        await _dbContext.ProductBrands.AddRangeAsync(productBrands);
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    //var productTypesData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\DataSeed\types.json");
                    var productTypesData = File.OpenRead(@"..\Infrastructure\Presistence\Data\DataSeed\types.json");
                    var productTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypesData);
                    if (productTypes is not null && productTypes.Any())
                        await _dbContext.ProductTypes.AddRangeAsync(productTypes);
                }
                if (!_dbContext.Products.Any())
                {
                    //  var productsData =await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\DataSeed\products.json");
                    var productsData = File.OpenRead(@"..\Infrastructure\Presistence\Data\DataSeed\products.json");
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(productsData);
                    if (products is not null && products.Any())
                        await _dbContext.Products.AddRangeAsync(products);
                }
                if (!_dbContext.Set<DeliveryMethod>().Any())
                {
                    //  var productsData =await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\DataSeed\products.json");
                    var DeliveryMethodData = File.OpenRead(@"..\Infrastructure\Presistence\Data\DataSeed\delivery.json");
                    var DeliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliveryMethodData);
                    if (DeliveryMethods is not null && DeliveryMethods.Any())
                        await _dbContext.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethods);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                // To Do
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    //Create roles
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        Email = "mohamedkilaney2@gmail.com",
                        DisplayName = "Mohamed Kilany",
                        UserName = "MohamedKilaney",
                        PhoneNumber = "01095678452"

                    };
                    var user02 = new ApplicationUser()
                    {
                        Email = "Aboseada@gmail.com",
                        DisplayName = "Aboseada",
                        UserName = "Aboseada",
                        PhoneNumber = "01095678453"
                    };

                    //Add users to the database with password
                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    //Assign roles to the users
                    await _userManager.AddToRoleAsync(user01, "Admin");
                    await _userManager.AddToRoleAsync(user02, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {

                //To Do
            }
        }
    }
}
