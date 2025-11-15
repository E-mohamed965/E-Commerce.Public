using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.BasketModule
{
    public class BasketDto
    {
        public string Id { get; set; } = null!;
        public ICollection<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; } //Foreign Key
        public decimal ShippingPrice { get; set; }
    }
}
