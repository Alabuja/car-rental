using CarRental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Data
{
    public class CarRentalDbContext : IdentityDbContext<CarRentalUser>
    {

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// Discount model DbSet
        /// </summary>
        public DbSet<Car> Cars { get; set; }
        public DbSet<CustomerBooking> CustomerBookings { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}