﻿using System;
using DDDTemplate.Api.Controllers.Base;
using DDDTemplate.Infrastructure.Response;
using DDDTemplate.Infrastructure.Response.Base;
using DDDTemplate.Infrastructure.Response.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DDDTemplate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : BaseController
    {
        private readonly IResponseService _responseService;

        public ErrorController(IResponseService responseService)
        {
            this._responseService = responseService;
        }

        [AllowAnonymous]
        [Route("/error-local-development")]
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

            if (exception is ArgumentNullException) { statusCode = 400; errorCode = ErrorCodes.NULL_ARGUMENT; }
            if (exception is ArgumentException) { statusCode = 400; errorCode = ErrorCodes.INVALID_REQUEST; }
            else if (exception is ApplicationException) { statusCode = 400; errorCode = ErrorCodes.SERVICE_NOT_AVAILABLE; }

            var serviceResult = this._responseService.FailedResponse(errorCode, exception.Message);

            return StatusCode(statusCode, serviceResult);
        }
    }
}