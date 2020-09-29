using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Models;
using CarRental.Models.InputModel;
using CarRental.Services;
using CarRental.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CarRental.Helpers;

namespace CarRental.Controllers
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<CarRentalUser> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<CarRentalUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;
        private readonly ExceptionService _exceptionService;
        private readonly CarRentalDbContext _context;


        public AuthController(SignInManager<CarRentalUser> signInManager,
         ILogger<AuthController> logger, 
         UserManager<CarRentalUser> userManager, 
         IConfiguration config,
         IAccountService accountService,
         ExceptionService exceptionService,
         CarRentalDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _config = config;
            _accountService = accountService;
            _exceptionService = exceptionService;
            _context = context;
        }


        /// <summary>
        /// Endpoint to Log a user in
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null) throw new KeyNotFoundException("Invalid Login Credentials");

                //Check if User Email is confirmed
                // if(!user.EmailConfirmed) throw new KeyNotFoundException("Confirm Email to Login");

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded) throw new KeyNotFoundException("Invalid Password");

                var token = await _accountService.CreateToken(user);
                _logger.LogInformation($"Created token for {user.UserName}");

                return Ok(new ResponseMessage{
                    Data = new 
                        {   
                            AccessToken = token, 
                            Email = user.Email,
                            EmailConfirmed = user.EmailConfirmed
                        },
                    Status = true
                });
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }
    }
}