using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

namespace E_Commerce.Web.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
        {
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer" , new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type=SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = "Enter 'Bearer' Followed by space and your token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[] {}
                    }
                });
            });
            return Services;
        }
        public static IServiceCollection AddWebApplicationServices(this IServiceCollection Services)
        {
            Services.Configure<ApiBehaviorOptions>((options) =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorsResponse;
            });
            return Services;
        }
        public static IServiceCollection AddJWTServices(this IServiceCollection Services,IConfiguration _configuration) {
            Services.AddAuthentication((config) =>
            {
                 config.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                 config.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWTOptions:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = _configuration["JWTOptions:Audience"],

                    ValidateLifetime= true,

                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTOptions:SecretKey"]))
                };
            });
            return Services;
        }
       
    }
}
