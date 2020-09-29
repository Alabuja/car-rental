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

    public class CarController : ControllerBase
    {
        private readonly CarService _carService;
        private readonly UserManager<CarRentalUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ExceptionService _exceptionService;
        private readonly IMapper _mapper;

        public CarController(
            UserManager<CarRentalUser> userManager,
            IConfiguration config,
            ExceptionService exceptionService,
            CarService carService,
            IMapper mapper)
        {
            _userManager = userManager;
            _config = config;
            _exceptionService = exceptionService;
            _carService = carService;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint to Get Cars
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public async Task<ActionResult<List<CarOutModel>>> GetCars()
        {
            try
            { 
                var request = await _carService.GetCars(
                    User
                );

                return _mapper.Map<List<CarOutModel>>(request);
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }

        /// <summary>
        /// Endpoint to Get car by Id
        /// </summary>
        /// /// <param name="carId"></param>
        /// <returns></returns>
        [HttpGet("{carId}")]
        [Authorize]
        public async Task<ActionResult<CarOutModel>> GetCarById(Guid carId)
        {
            try
            { 
                var request = await _carService.GetCarById(
                    User,
                    carId
                );

                return _mapper.Map<CarOutModel>(request);
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }

        /// <summary>
        /// Endpoint to Add a Car
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CarOutModel>> AddCar([FromBody] CarInputModel model)
        {
            try
            { 
                var request = await _carService.AddCar(
                    User,
                    model
                );

                return _mapper.Map<CarOutModel>(request);
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }

    }
}