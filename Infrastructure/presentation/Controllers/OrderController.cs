using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace presentation.Controllers
{
    [Authorize]
    public class OrderController(IServiceManager _serviceManager) : ApiBaseController
    {
        
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var email= User.FindFirstValue(ClaimTypes.Email);
            var orderToReturnDto = await _serviceManager.OrderService.CreateOrderAsync(orderDto, email);
            return Ok(orderToReturnDto);
        }
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var delMethods= await _serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(delMethods);
        }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrdersByEmail()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var Orders= await _serviceManager.OrderService.GetAllOrdersAsync(email);
            return Ok(Orders);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
    }
}
