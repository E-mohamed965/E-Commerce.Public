using DomainLayer.Models.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class ProductCountSpecification : BaseSpecification<Product, int>
    {
        public ProductCountSpecification(ProductQueryParams productQueryParams) : base(
             p =>
             (!productQueryParams.brandId.HasValue || p.BrandId == productQueryParams.brandId) &&
             (!productQueryParams.typeId.HasValue || p.TypeId == productQueryParams.typeId) &&
             (string.IsNullOrWhiteSpace(productQueryParams.SearchValue) || p.Name.ToLower().Contains(productQueryParams.SearchValue.ToLower()))
            )
        {
            
        }
    }
}
