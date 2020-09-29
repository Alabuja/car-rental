using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class CustomerBooking
    {
        public Guid Id { get;set; }
        public Guid CustomerId { get;set; }
        public Customer Customer { get;set; }
        public DateTime StartTime { get; set; }
        public DateTime DueTime { get; set; }
        public Guid CarId { get; set; }
        public Car Car { get; set; }
    }
}
