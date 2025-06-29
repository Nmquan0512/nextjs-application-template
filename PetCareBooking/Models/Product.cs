using System.ComponentModel.DataAnnotations;

namespace PetCareBooking.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
