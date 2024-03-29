﻿using Application.Contracts.Auth.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (JwtMiddlewareDto)context.HttpContext.Items["User"];
            if (user == null)
            {
                // not logged in
                var unAuthorizedMessage = new
                {
                    status = 0,
                    message = "Unauthorized",
                    exception = "Unauthorized"
                };

                context.Result = new JsonResult(unAuthorizedMessage) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
