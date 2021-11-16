﻿using System.Threading.Tasks;
using DDDTemplate.Api.Controllers.Base;
using DDDTemplate.Application.Abstraction.User;
using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Infrastructure.Response.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DDDTemplate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IServiceResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn([FromBody] UserLoginDto model)
        {
            var response = await this._authenticationService.SignInAsync(model);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IServiceResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterDto model)
        {
            var response = await this._authenticationService.SignUpAsync(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IServiceResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var response = await this._authenticationService.ForgotPasswordAsync(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IServiceResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var response = await this._authenticationService.ResetPasswordAsync(model);
            return Ok(response);
        }

        [Attributes.Authorize]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IServiceResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult ValidateToken()
        {
            var result = this._authenticationService.ValidateToken(new UserTokenDto() { Token = this.RequesterToken });

            return Ok(result);
        }

        [Attributes.Authorize]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IServiceResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SendActivation()
        {
            var result = await this._authenticationService.ReSendActivationEmailAsync(new UserIdDto() { Id = RequesterId.Value });

            return Ok(result);
        }
    }
}
