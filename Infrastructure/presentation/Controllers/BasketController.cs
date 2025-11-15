using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager _service) : ControllerBase
    {
        //Get Basket
        [HttpGet] // GET : baseUrl/api/basket/{BasketId}
        public async Task<ActionResult<BasketDto>> GetBasketByIdAsync(string BasketId)
        {
            var basket = await _service.BasketService.GetBasketAsync(BasketId);
            return Ok(basket);
        }
        //Create Or Update Basket
        [HttpPost] // POST : baseUrl/api/basket
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasketAsync([FromBody] BasketDto basket)
        {
            var createdOrUpdatedBasket = await _service.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(createdOrUpdatedBasket);
        }
        //Delete Basket
        [HttpDelete] // DELETE : baseUrl/api/basket/{BasketId}
        public async Task<IActionResult> DeleteBasketByIdAsync(string BasketId)
        {
           var result= await _service.BasketService.DeleteBasketAsync(BasketId);
            return Ok(result);
        }

    }
}
