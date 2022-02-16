using Application.Abstraction.Interfaces;
using Application.Abstraction.User;
using Infrastructure.Security.Token.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Api.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;        

        public JwtMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;            
        }

        public async Task Invoke(HttpContext context, IAuthenticationService authService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachUserToContext(context, authService, token).ConfigureAwait(false);

            await _next(context).ConfigureAwait(false);
        }

        private async Task AttachUserToContext(HttpContext context, IAuthenticationService authService, string token)
        {
            try
            {
                var jwtToken = (JwtSecurityToken)this._tokenService.GetToken(token);
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);               
                var jwtUserResult = await authService.GetJwtUserbyIdAsync(
                    new Application.Contracts.Auth.Request.UserIdDto() { Id = userId }).ConfigureAwait(false);
                if (jwtUserResult != null)
                {
                    var jwtUser = jwtUserResult.Data;
                    context.Items["User"] = jwtUser;
                }
            }
            catch
            {
            }
        }
    }
}
