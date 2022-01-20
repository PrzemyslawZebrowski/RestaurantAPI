using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class AddDishDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
