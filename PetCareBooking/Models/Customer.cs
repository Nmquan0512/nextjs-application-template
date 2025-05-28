using System.ComponentModel.DataAnnotations;

namespace PetCareBooking.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
