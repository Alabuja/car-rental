using System;

namespace CarRental.Models.OutputModel
{
    public class CustomerBookingOutputModel
    {
        public CarOutModel CarOutModel { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DueTime { get; set; }
    }
}