using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRental.Models;

namespace CarRental.Services
{
    public interface IAccountService
    {
         Task<List<Claim>> GetValidClaims(CarRentalUser user);
         Task<string> CreateToken(CarRentalUser user);
    }
}