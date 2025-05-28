using System;
using System.ComponentModel.DataAnnotations;

namespace PetCareBooking.Models
{
    public class Voucher
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public decimal DiscountAmount { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
