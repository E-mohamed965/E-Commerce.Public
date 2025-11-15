using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;
using Stripe;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class PaymentService(IConfiguration _configuration,
                                  IBasketRepository _basketRepository,
                                  IUnitOfWork _unitOfWork,
                                  IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];

            var basket = await  _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
            {
                
                throw new BasketNotFoundException(basketId);
            }
            var productRepo = _unitOfWork.GetRepositoryAsync<DomainLayer.Models.ProductModule.Product, int>();  
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if(product is null)
                {
                    throw new ProductNotFoundException(item.Id);
                }
                item.Price = product.Price;
            }
            if(basket.DeliveryMethodId is null)
            {
                throw new ArgumentNullException();
            }
            var deliveryMethod = await _unitOfWork.GetRepositoryAsync<DomainLayer.Models.OrderModule.DeliveryMethod, int>()
                                               .GetByIdAsync(basket.DeliveryMethodId.Value);
            if(deliveryMethod is  null)
            {
               throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            }
            basket.ShippingPrice = deliveryMethod.Cost;
            var amount = (long)basket.Items.Sum(i => i.Quantity * i.Price ) + (long)(basket.ShippingPrice );
            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount * 100,
                    Currency = "AED",
                    PaymentMethodTypes = ["card"]
                };
                var intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amount * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
             await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return _mapper.Map<Basket, BasketDto>(basket);  
        }

        public async Task UpdateOrderPaymentStatusAsync(string request, string stripeHeader)
        {
            var stripeEvent = EventUtility.ConstructEvent(
                request,
                stripeHeader,
                _configuration.GetSection("Stripe")["EndPointSecret"]
            );
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    {
                        var intent = (PaymentIntent)stripeEvent.Data.Object;
                        //Update Order Status to Payment Received
                       
                        await UpdatePaymentRecievedAsync(intent.Id);

                        break;
                    }
                case EventTypes.PaymentIntentPaymentFailed:
                    {
                        var intent = (PaymentIntent)stripeEvent.Data.Object;
                        //Update Order Status to Payment Failed
                        await UpdatePaymentFailedAsync(intent.Id);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unhandled Stripe Event Type", stripeEvent.Type);
                        break;
                    }
            }
        }
        private async Task UpdatePaymentRecievedAsync(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.GetRepositoryAsync<DomainLayer.Models.OrderModule.Order, Guid>()
                                        .GetByIdAsync(spec);
            order.OrderStatus = DomainLayer.Models.OrderModule.OrderStatus.PaymentRecieved;
            _unitOfWork.GetRepositoryAsync<DomainLayer.Models.OrderModule.Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task UpdatePaymentFailedAsync(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.GetRepositoryAsync<DomainLayer.Models.OrderModule.Order, Guid>()
                                        .GetByIdAsync(spec);
            order.OrderStatus = DomainLayer.Models.OrderModule.OrderStatus.PaymentFailed;
            _unitOfWork.GetRepositoryAsync<DomainLayer.Models.OrderModule.Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
