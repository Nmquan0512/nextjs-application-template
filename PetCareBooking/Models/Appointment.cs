using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareBooking.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required]
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";
    }
}
