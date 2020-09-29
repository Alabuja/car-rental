using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Models.InputModel;
using CarRental.Models.OutputModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarRental.Services
{
    public class CustomerBookingService
    {
        private readonly UserManager<CarRentalUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly CarRentalDbContext _context;

        public CustomerBookingService(UserManager<CarRentalUser> userManager, IConfiguration configuration, CarRentalDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }

        public async Task<CustomerBooking> RentCar(
            CustomerBookingInputModel model)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(u => u.Id == model.CarId);

            if(car != null) throw new KeyNotFoundException("Car not found");

            if(car.IsRented) throw new ConflictException("Car is already rented");

            var customer = await _context.Customers
                .Where(u => u.Email == model.Email || u.PhoneNumber == model.PhoneNumber)
                .FirstOrDefaultAsync();

            if(customer == null)
            {
                var newCustomer = new Customer
                {
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Fullname = model.Fullname
                };

                await _context.Customers.AddAsync(newCustomer);
                await _context.SaveChangesAsync();
            }
            
            var mainCustomer = await _context.Customers
                .Where(u => u.Email == model.Email || u.PhoneNumber == model.PhoneNumber)
                .FirstOrDefaultAsync();

            var newCar = await _context.Cars
                .FirstOrDefaultAsync(u => u.Id == model.CarId);

            newCar.IsRented = true;
            _context.Cars.Update(newCar);
            await _context.SaveChangesAsync();

            var newBooking = CreateNewBooking(model, mainCustomer.Id);

            await _context.CustomerBookings.AddAsync(newBooking);
            await _context.SaveChangesAsync();

            return newBooking;
        }

         public async Task<VehicleRentedOutputModel> GetTotal(
            ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) throw new KeyNotFoundException("User not found");
            
            var time = DateTime.Now;
            var bookings = await _context.CustomerBookings.Where(p => p.DueTime >= time).ToListAsync();

            double total = 0;

            foreach (var item in bookings)
            {
                var carId = item.CarId;

                var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == carId);

                var amount = car.Price;
                total = total + amount;
            }

            var vehicle = new VehicleRentedOutputModel
            {
                TotalAmount = total,
                NumberOfVehicle = bookings.Count()
            };

            return vehicle;
        }

        private CustomerBooking CreateNewBooking(CustomerBookingInputModel model, Guid customerId)
        {
            var customerBooking = new CustomerBooking
            {
                StartTime = model.StartTime,
                DueTime = model.DueTime,
                CarId = model.CarId,
                CustomerId = customerId
            };

            return customerBooking;
        } 
    }
}