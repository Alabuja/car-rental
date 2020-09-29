using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using CarRental.Exceptions;

namespace CarRental.Services
{
    /// <summary>
    /// handles exceptions
    /// </summary>
    public class ExceptionService
    {
        private readonly IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExceptionService(
            IHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        protected object RefineException(Exception ex)
        {
            return new
            {
                message = ex.Message,
                stackTrace =  ex.StackTrace,
                data = ex.Data,
                innerException = (ex.InnerException != null) ? RefineException(ex.InnerException) : null
            };
        }

        public ActionResult GetActionResult(Exception ex)
        {
            var refinedEx = RefineException(ex);
            if (ex is KeyNotFoundException)
            {
                return new NotFoundObjectResult(refinedEx);
            }
            else if (ex is ConflictException)
            {
                return new ConflictObjectResult(refinedEx);
            }

            throw ex;
        }
    }
}
