using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDTemplate.Application.Abstraction.Authentication;
using DDDTemplate.Application.Contracts.Auth.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DDDTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }


        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRegisterDto model)
        {

            var response = await this._authenticationService.SignUpAsync(model);
            return Ok(response);

        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {

            var response = await this._authenticationService.ForgotPasswordAsync(model);
            return Ok(response);

        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
