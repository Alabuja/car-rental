using AutoMapper;
using CarRental.Models.OutputModel;
using CarRental.Models;

namespace CarRental
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Automapper profile constructor
        /// </summary>
        public AutoMapperProfile()
        {
            // User
            CreateMap<Car, CarOutModel>()
                .ReverseMap();

            CreateMap<CustomerBooking, CustomerBookingOutputModel>()
                .ReverseMap();

            CreateMap<Customer, CustomerOutputModel>()
                .ReverseMap();
        }
    }
}