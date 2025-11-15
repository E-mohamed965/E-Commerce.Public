using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }
        //[ForeignKey(nameof(ProductBrand))]
        public int BrandId { get; set; }
      //  [ForeignKey(nameof(ProductType))]
        public int TypeId { get; set; }
        public virtual ProductBrand ProductBrand { get; set; } = null!;
        public virtual ProductType ProductType { get; set; } = null!;

    }
}
