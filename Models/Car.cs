using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class Car
    {
        public Guid Id { get; set; }
        public string CarModel { get; set; }
        public string Platenumber { get; set; }
        public bool IsRented { get; set; }
        public double Price { get; set; }
    }
}
