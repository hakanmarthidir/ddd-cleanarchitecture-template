using Application.Contracts.Auth.Response;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers.Base
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
