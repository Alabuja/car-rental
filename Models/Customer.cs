using System;
using System.Collections.Generic;

namespace CarRental.Models
{
    public class Customer
    {
        public Guid Id { get;set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<CustomerBooking> CustomerBookings { get; set; }
    }
}