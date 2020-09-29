using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarRental.Services
{
    public class CustomerService
    {
        private readonly UserManager<CarRentalUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly CarRentalDbContext _context;

        public CustomerService(UserManager<CarRentalUser> userManager, IConfiguration configuration, CarRentalDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IQueryable<Customer>> GetQueryable(
            ClaimsPrincipal User
        )
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) throw new KeyNotFoundException("User not found");

            var query = _context.Customers
                                .Include(c => c.CustomerBookings)
                                .ThenInclude(c => c.Car)
                                .Where(v => v.Email != null);

            return query;
        }

        public async Task<List<Customer>> GetCustomers(
            ClaimsPrincipal User
        )
        {
            var query = await GetQueryable(User);

            var customers = await query.ToListAsync();

            return customers;
        }

        public async Task<Customer> GetCustomerByEmail(
            ClaimsPrincipal User,
            string email)
        {
            var query = await GetQueryable(User);

            var customer = await query.Include(p => p.CustomerBookings)
                            .ThenInclude(s => s.Car)
                            .FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null) throw new KeyNotFoundException("Customer not found");

            return customer;
        }
    }
}