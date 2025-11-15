
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using presentation.Data.Contexts;
using Presistence;
using Presistence.Repositories;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using Shared.ErrorModels;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();
            builder.Services.AddJWTServices(builder.Configuration);
            //builder.Services.AddRateLimiter(options =>
            //{
            //    options.AddFixedWindowLimiter("fixed", config =>
            //    {
            //        config.Window = TimeSpan.FromSeconds(100);
            //        config.PermitLimit = 5;           // 5 requests max
            //        config.QueueLimit = 0;            // no queued requests
            //        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    });
            //});
            #endregion

            var app = builder.Build();
          
            await app.SeedDataAsync();



            #region Configure the HTTP request pipeline.

           // app.UseRateLimiter();

            app.UseCustomExceptionMiddleWare();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
