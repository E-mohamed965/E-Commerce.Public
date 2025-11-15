using Microsoft.Extensions.DependencyInjection;
using Service.MappingProfiles;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddAutoMapper(config => config.AddProfile(new ProductProfile()), typeof(AssemblyReference).Assembly);
            
            Services.AddScoped<IServiceManager, ServiceManager>();

            Services.AddScoped<IBasketService, BasketService>();
            Services.AddScoped<IOrderService, OrderService>();  
            Services.AddScoped<IProductService,ProductService>();
            Services.AddScoped<IAuthenticationService,AuthenticationService>();
            Services.AddScoped<IPaymentService,PaymentService>();

            Services.AddScoped<ICacheService,CacheService>();

            Services.AddScoped<Func<IProductService>>(provider =>
            {
                return () => provider.GetRequiredService<IProductService>();
            });
            Services.AddScoped<Func<IPaymentService>>(provider =>
            {
                return () => provider.GetRequiredService<IPaymentService>();
            });
            Services.AddScoped<Func<IAuthenticationService>>(provider =>
            {
                return () => provider.GetRequiredService<IAuthenticationService>();
            });
            Services.AddScoped<Func<IOrderService>>(provider =>
            {
                return () => provider.GetRequiredService<IOrderService>();
            });
            Services.AddScoped<Func<IBasketService>>(provider =>
            {
                return () => provider.GetRequiredService<IBasketService>();
            });
            return Services;
        }
    }
}
