using Microsoft.EntityFrameworkCore;
using PetCareBooking.Models;

namespace PetCareBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
    }
}
