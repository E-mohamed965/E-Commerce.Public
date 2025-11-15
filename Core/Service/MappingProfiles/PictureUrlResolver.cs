using AutoMapper;
using DomainLayer.Models.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
            else
                return string.Empty;
        }
    }
}
