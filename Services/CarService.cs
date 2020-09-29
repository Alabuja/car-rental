using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Models.InputModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarRental.Services
{
    public class CarService
    {
        private readonly UserManager<CarRentalUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly CarRentalDbContext _context;

        public CarService(UserManager<CarRentalUser> userManager, IConfiguration configuration, CarRentalDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }

        public IQueryable<Car> GetQueryable(
            ClaimsPrincipal User
        )
        {

            var query = _context.Cars.Where(v => v.Platenumber != null);

            return query;
        }

        public async Task<List<Car>> GetCars(
            ClaimsPrincipal User
        )
        {
            var query = GetQueryable(User);

            var customers = await query.ToListAsync();

            return customers;
        }

        public async Task<Car> GetCarById(
            ClaimsPrincipal User,
            Guid carId)
        {
            var query = GetQueryable(User);

            var car = await query
                            .FirstOrDefaultAsync(c => c.Id == carId);

            if (car == null) throw new KeyNotFoundException("Car not found");

            return car;
        }

        public async Task<Car> AddCar(ClaimsPrincipal User,
            CarInputModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) throw new KeyNotFoundException("User not found");

            var car = await _context.Cars
                .FirstOrDefaultAsync(u => u.Platenumber == model.Platenumber);

            if(car != null) throw new ConflictException("Car with plate number already exist");

            var newCar = new Car
            {
                Platenumber = model.Platenumber,
                CarModel = model.CarModel,
                Price = model.Price
            };

            await _context.Cars.AddAsync(newCar);
            await _context.SaveChangesAsync();

            return newCar;
        }

        public async Task DeleteCar(ClaimsPrincipal User,
            Guid carId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) throw new KeyNotFoundException("User not found");
            
            var dbCar = await _context.Cars.FirstOrDefaultAsync(v => v.Id == carId);
            if (dbCar == null)
            {
                throw new KeyNotFoundException("Car not found");
            }
            
            var car = await _context.CustomerBookings
                .FirstOrDefaultAsync(u => u.CarId == carId);

            if(car != null) throw new ConflictException("Car is already being booked");
            
            _context.Cars.Remove(dbCar);
            await _context.SaveChangesAsync();
        }
    }
}