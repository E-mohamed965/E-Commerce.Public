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
    public class PaymentsController(IServiceManager serviceManager) : ApiBaseController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdate(string basketId)
        {
           var basket = await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(basket);
        }
        [HttpPost("WebHook")]
        public async Task<IActionResult> WebHook()
        {
            var json= await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            await  serviceManager.PaymentService.UpdateOrderPaymentStatusAsync(json, Request.Headers["Stripe-Signature"]);          
            return new EmptyResult();
        }
    }
}
