using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            var Scope = app.Services.CreateScope();

            var seed = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await seed.DataSeedAsync();
            await seed.IdentityDataSeedAsync();
        }

        public static IApplicationBuilder UseCustomExceptionMiddleWare(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExceptionHandlerMiddleWare>();
        }
        public static IApplicationBuilder UseSwaggerMiddleWares(this IApplicationBuilder app)
        {

            app.UseSwagger();
            app.UseSwaggerUI(options=>
            {
                options.ConfigObject = new ConfigObject()
                {
                    DisplayRequestDuration = true
                };
                options.DocumentTitle = "E_Commerce_API_Route_DotNet";

                options.JsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                options.DocExpansion(DocExpansion.None);

                options.EnableFilter();
                options.EnablePersistAuthorization();
            });

            return app;
        }
       
    }
}