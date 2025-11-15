using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shared;
using Shared.DataTransferObjects.ProductModule;
using Microsoft.AspNetCore.Authorization;
using Shared.ErrorModels;
using presentation.Attributes;

namespace presentation.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager):ControllerBase
    {
        
        [HttpGet]
        [Cache(300)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams productQueryParams)
        {
            var products = await _serviceManager.ProductService.GetAllProductAsync(productQueryParams);
            return Ok(products);
        }
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorToReturn),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            if (product == null)
                return null;
            return Ok(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
    }
}
