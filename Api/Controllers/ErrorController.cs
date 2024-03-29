﻿using Api.Controllers.Base;
using Application.Abstraction.Interfaces;
using Application.Abstraction.Response;
using Application.Abstraction.Response.Enums;
using Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : BaseController
    {

        private readonly ILogService<ErrorController> _logger;
        public ErrorController(ILogService<ErrorController> logger)
        {
            this._logger = logger;
        }

        [AllowAnonymous]
        [Route("/error-local-development")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
            }

            return this.GetExceptionResult();
        }

        [AllowAnonymous]
        [Route("/error")]
        [ProducesResponseType(typeof(IServiceResponse), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            return this.GetExceptionResult();
        }

        private IActionResult GetExceptionResult()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var statusCode = 500;
            var errorCode = ErrorCodes.UNKNOWN_ERROR;

            this._logger.LogError(exception, "An unknown error occurred..");

            if (exception is ArgumentNullException) { statusCode = 400; errorCode = ErrorCodes.NULL_ARGUMENT; }
            if (exception is ArgumentException) { statusCode = 400; errorCode = ErrorCodes.INVALID_REQUEST; }
            else if (exception is ApplicationException) { statusCode = 400; errorCode = ErrorCodes.SERVICE_NOT_AVAILABLE; }

            var serviceResult = ServiceResponse.Failure(errorCode, exception.Message);

            return StatusCode(statusCode, serviceResult);
        }
    }
}
