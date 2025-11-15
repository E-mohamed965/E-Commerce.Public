using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BasketService(IBasketRepository _basketRepository , IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var basketModle = _mapper.Map<BasketDto, Basket>(basket);
            var CreatedOrUpdatedBasket =  await _basketRepository.CreateOrUpdateBasketAsync(basketModle);
            if(CreatedOrUpdatedBasket is not null)
            return await GetBasketAsync(basket.Id);
            else 
                throw new Exception("Failed to create or update basket , Try Again Later");
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }

        public async Task<BasketDto> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null)
                throw new BasketNotFoundException(basketId);
            return _mapper.Map<Basket,BasketDto>(basket);

        }
    }
}
