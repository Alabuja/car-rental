using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CarRental.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Data
{
    public class DbInitializer
    {
        public static async Task Seed(CarRentalDbContext context, UserManager<CarRentalUser> _userManager)
        {
            Console.WriteLine("Seeding Database with Users");

            if (!context.Users.Any())
            {
                string password = "Pass123$";
                var _faker = new Faker();
                var users = new CarRentalUser[] 
                {
                    new CarRentalUser
                    {
                        UserName = "daniel@mailinator.com", // admin
                        Email = "daniel@mailinator.com",
                        EmailConfirmed = true,
                        NickName = "Alatech",
                        PhoneNumber = _faker.Phone.PhoneNumber(),
                        PhoneNumberConfirmed = true,
                    },
                    new CarRentalUser
                    {
                        UserName = "alatech@mailinator.com",
                        Email = "alatech@mailinator.com",
                        EmailConfirmed = true,
                        NickName = "Daniel",
                        PhoneNumber = _faker.Phone.PhoneNumber(),
                        PhoneNumberConfirmed = true,
                    }
                    
                };

                foreach (var user in users)
                {
                    var dbUser = await _userManager.FindByEmailAsync(user.Email);
                    if (dbUser == null)
                    {
                        var result = await _userManager.CreateAsync(user, password);

                        if (result.Succeeded)
                        {
                            Console.WriteLine($"User {user.UserName} created");
                        }
                        
                    }
               }
            }
        }
    }
}