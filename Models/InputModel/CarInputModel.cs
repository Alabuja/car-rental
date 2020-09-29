using System;

namespace CarRental.Models.InputModel
{
    public class CarInputModel
    {
        public Guid Id { get; set; }
        public string CarModel { get; set; }
        public string Platenumber { get; set; }
        public bool IsRented { get; set; }
        public double Price { get; set; }
    }
}