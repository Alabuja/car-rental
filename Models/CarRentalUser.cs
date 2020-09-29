using Microsoft.AspNetCore.Identity;

namespace CarRental.Models
{
    public class CarRentalUser : IdentityUser
    {
        public string NickName { get; set; }
    }
}