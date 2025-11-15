using DomainLayer.Models.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class ProductWithBrandAndTypeSpecification : BaseSpecification<Product,int>
    {
        public ProductWithBrandAndTypeSpecification(ProductQueryParams productQueryParams) : base(
             p=>
             (!productQueryParams.brandId.HasValue||p.BrandId== productQueryParams.brandId)&&
             (!productQueryParams.typeId.HasValue||p.TypeId== productQueryParams.typeId)&&
             (string.IsNullOrWhiteSpace(productQueryParams.SearchValue)||p.Name.ToLower().Contains(productQueryParams.SearchValue.ToLower()))
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            switch(productQueryParams.sortingOption)
            {
                case ProductSortingOption.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOption.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOption.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOption.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    //AddOrderBy(p => p.Name);
                    break;
            }

            ApplyPagination(productQueryParams.PageSize, productQueryParams.PageIndex);

        }
        public ProductWithBrandAndTypeSpecification(int Id) : base(p => p.Id == Id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType); 
        }
    }
}
