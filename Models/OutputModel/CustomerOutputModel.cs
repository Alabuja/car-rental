using System;
using System.Collections.Generic;

namespace CarRental.Models.OutputModel
{
    public class CustomerOutputModel
    {
        public Guid Id { get;set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<CustomerBookingOutputModel> CustomerBookings { get; set; }
    }
}