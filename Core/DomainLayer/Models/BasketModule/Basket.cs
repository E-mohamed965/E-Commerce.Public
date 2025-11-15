using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.BasketModule
{
    public class Basket
    {
        public string Id { get; set; } //GUID, Created from Client [Frontend]
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public string? PaymentIntentId { get; set; }    
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; } //Foreign Key
        public decimal ShippingPrice { get; set; }

    }
}
