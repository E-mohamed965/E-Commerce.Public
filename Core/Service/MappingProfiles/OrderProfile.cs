using AutoMapper;
using DomainLayer.Models.OrderModule;
using Shared.DataTransferObjects.IdentityModule;
using Shared.DataTransferObjects.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
            CreateMap<AddressDto, OrderAddress>()
                .ForMember(dist=>dist.Region,options=>options.MapFrom(src=>src.Country))
                .ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                             .ForMember(dist=>dist.DeliveryMethod, option=>option.MapFrom(src=>src.DeliveryMethod.ShortName));
            CreateMap<OrderItem, OrderItemDto>()
                          .ForMember(dist => dist.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                          .ForMember(dist=>dist.PictureUrl,options=>options.MapFrom<OrderItemPictureUrlResolver>());
                          
        }
    }
}
