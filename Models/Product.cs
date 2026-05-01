using System.ComponentModel.DataAnnotations;

namespace RolesDemo.Models
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
