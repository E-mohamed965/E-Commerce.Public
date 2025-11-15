using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects.BasketModule
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        [Range(1,double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        [Range(1, 100, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}