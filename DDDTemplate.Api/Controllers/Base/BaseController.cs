using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Auth.Response;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DDDTemplate.Api.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid? RequesterId
        {
            get
            {
                return ((JwtMiddlewareDto)HttpContext.Items["User"]).Id;
            }
        }
        protected string RequesterToken
        {
            get
            {
                return HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            }
        }
    }
}
