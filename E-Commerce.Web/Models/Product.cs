using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Web.Models
{
    public class Product
    {
        [Range(10,1000)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
    }
}
