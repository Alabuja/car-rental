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
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly UserManager<CarRentalUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ExceptionService _exceptionService;
        private readonly IMapper _mapper;

        public CustomerController(
            UserManager<CarRentalUser> userManager,
            IConfiguration config,
            ExceptionService exceptionService,
            CustomerService customerService,
            IMapper mapper)
        {
            _userManager = userManager;
            _config = config;
            _exceptionService = exceptionService;
            _customerService = customerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint to Get customers
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<CustomerOutputModel>>> GetCustomers()
        {
            try
            { 
                var request = await _customerService.GetCustomers(
                    User
                );

                return _mapper.Map<List<CustomerOutputModel>>(request);
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }

        /// <summary>
        /// Endpoint to Get a customer
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        
        [HttpGet("{email}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<CustomerOutputModel>> GetCustomerById(string email)
        {
            try
            { 
                var request = await _customerService.GetCustomerByEmail(
                    User,
                    email
                );

                return _mapper.Map<CustomerOutputModel>(request);
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }
        
    }
}
