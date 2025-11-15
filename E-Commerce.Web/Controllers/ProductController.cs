using E_Commerce.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            return new Product { Id = id };
        }
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return new List<Product> { new Product { Id = 1}, new Product { Id = 2} };
        }
        [HttpPost]
        public ActionResult<Product> Create(Product product)
        {
            return product;
        }
        [HttpPut]
        public ActionResult<Product> Update(Product product)
        {
            return product;
        }
        [HttpDelete]
        public ActionResult<Product> Delete(Product product)
        {
            return product;
        }
    }
}
