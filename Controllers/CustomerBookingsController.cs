using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.Models;
using CarRental.Models.OutputModel;
using CarRental.Models.InputModel;
using CarRental.Services;
using CarRental.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CarRental.Controllers
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class CustomerBookingsController : ControllerBase
    {
        private readonly CustomerBookingService _customerBooking;
        private readonly UserManager<CarRentalUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ExceptionService _exceptionService;
        private readonly IMapper _mapper;

        public CustomerBookingsController(
            UserManager<CarRentalUser> userManager,
            IConfiguration config,
            ExceptionService exceptionService,
            CustomerBookingService customerBooking,
            IMapper mapper)
        {
            _userManager = userManager;
            _config = config;
            _exceptionService = exceptionService;
            _customerBooking = customerBooking;
            _mapper = mapper;
        }


        /// <summary>
        /// Endpoint to Rent a Car
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CustomerBookingOutputModel>> AddCar([FromBody] CustomerBookingInputModel model)
        {
            try
            { 
                var request = await _customerBooking.RentCar(
                    model
                );

                return _mapper.Map<CustomerBookingOutputModel>(request);
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }

        /// <summary>
        /// Endpoint to show total vehicle rented out and total amount received
        /// </summary>
        /// <returns></returns>
        [HttpGet("total")]
        public async Task<ActionResult<VehicleRentedOutputModel>> GetTotal()
        {
            try
            { 
                var request = await _customerBooking.GetTotal(
                    User
                );

                return request;
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }
    }
}