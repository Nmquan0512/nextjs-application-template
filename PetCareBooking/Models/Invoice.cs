using System;
using System.ComponentModel.DataAnnotations;

namespace PetCareBooking.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public string? VoucherCode { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal FinalAmount { get; set; }
    }
}
