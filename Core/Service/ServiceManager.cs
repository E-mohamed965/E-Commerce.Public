using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager(IUnitOfWork _unitOfWork,IMapper _mapper,IBasketRepository _basketRepository,UserManager<ApplicationUser> _userManager,IConfiguration _configuration):IServiceManager
    {
        private readonly Lazy<IProductService> _productService = new Lazy<IProductService>(()=>new ProductService(_unitOfWork,_mapper));
        private readonly Lazy<IBasketService> _basketService = new Lazy<IBasketService>(()=>new BasketService(_basketRepository,_mapper));
        private readonly Lazy<IAuthenticationService> _authenticationService = new Lazy<IAuthenticationService>(()=>new AuthenticationService(_userManager,_configuration,_mapper));
        private readonly Lazy<IOrderService> _orderService = new Lazy<IOrderService>(()=>new OrderService(_mapper,_basketRepository,_unitOfWork));
        private readonly Lazy<IPaymentService> _paymentService = new Lazy<IPaymentService>(() => new PaymentService(_configuration,_basketRepository,_unitOfWork,_mapper));

        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

        public IOrderService OrderService => _orderService.Value;

        public IPaymentService PaymentService => _paymentService.Value;
    }
}
