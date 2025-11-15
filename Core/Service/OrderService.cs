using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;
using Shared.DataTransferObjects.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class OrderService(IMapper _mapper , IBasketRepository _basketRepository , IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            //Map Adress Dto to Order Address
            var address = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);
            //Get Basket
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId);

            if (basket is null) throw new BasketNotFoundException(orderDto.BasketId);

            var existingOrderSpec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId!);
            var existingOrder = await _unitOfWork.GetRepositoryAsync<Order, Guid>().GetByIdAsync(existingOrderSpec);

            if(existingOrder is not null)
            {
                _unitOfWork.GetRepositoryAsync<Order, Guid>().Delete(existingOrder);
            }
            //Create OrderItem List
            List<OrderItem> orderItems = new List<OrderItem>();
            var prodRepo = _unitOfWork.GetRepositoryAsync<Product, int>();
            foreach (var item in basket.Items) {
                var product = await prodRepo.GetByIdAsync(item.Id);
                if (product is null) { 
                    throw new ProductNotFoundException(item.Id); }
                var orderItem = new OrderItem()
                {
                    Product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        PictureUrl = product.PictureUrl,
                        ProductName = product.Name
                    },
                    Price = product.Price,
                    Quantity = item.Quantity

                };
                orderItems.Add(orderItem);
            }
            //Get Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if(deliveryMethod is null)
                throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            //Calculate Sub Total
            var subTotal = orderItems.Sum(o => o.Price*o.Quantity);

            var order = new Order(email, address, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);
            await _unitOfWork.GetRepositoryAsync<Order,Guid>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<Order, OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var spec = new OrderSpecification(email);
            var orders = await _unitOfWork.GetRepositoryAsync<Order, Guid>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<Order>,IEnumerable<OrderToReturnDto>>(orders);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod>,IEnumerable< DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecification(id);
            var order = await _unitOfWork.GetRepositoryAsync<Order, Guid>().GetByIdAsync(spec);
            return _mapper.Map<Order, OrderToReturnDto>(order);
        }
    }
}
