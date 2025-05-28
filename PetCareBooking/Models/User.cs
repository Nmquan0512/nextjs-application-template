using System.ComponentModel.DataAnnotations;

namespace PetCareBooking.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Role { get; set; }
    }
}
