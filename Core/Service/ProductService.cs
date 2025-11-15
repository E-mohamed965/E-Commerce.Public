using AutoMapper;
using DomainLayer.Contracts;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.Exceptions;
using System.Threading.Tasks;
using DomainLayer.Models.ProductModule;
using Shared.DataTransferObjects.ProductModule;

namespace Service
{
    internal class ProductService(IUnitOfWork _unitOfWork,IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo= _unitOfWork.GetRepositoryAsync<ProductBrand, int>();
            var brands= await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductBrand>,IEnumerable<BrandDto>>(brands);
            

        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductAsync(ProductQueryParams productQueryParams)
        {
            var repo= _unitOfWork.GetRepositoryAsync<Product, int>(); 
            var spec=new ProductWithBrandAndTypeSpecification(productQueryParams);
            var specCount=new ProductCountSpecification(productQueryParams);
            var products= await repo.GetAllAsync(spec);
            var productsDto= _mapper.Map<IEnumerable<Product>,IEnumerable<ProductDto>>(products);
            var productCount= productsDto.Count();
            var totalCount= await repo.CountAsync(specCount);
            return new PaginatedResult<ProductDto>(productQueryParams.PageIndex, productCount,totalCount, productsDto);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
          
            var types= await _unitOfWork.GetRepositoryAsync<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var repo= _unitOfWork.GetRepositoryAsync<Product, int>();
            var spec=new ProductWithBrandAndTypeSpecification(id);
            var product= await repo.GetByIdAsync(spec);
            if(product ==null)
            {
                throw new ProductNotFoundException(id);
            }
            else
                return _mapper.Map<Product, ProductDto>(product);
        }
    }
}
