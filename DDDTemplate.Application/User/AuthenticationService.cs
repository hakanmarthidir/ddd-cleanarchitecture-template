using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Auth.Response;
using DDDTemplate.Infrastructure.Response.Base;
using DDDTemplate.Application.Abstraction.Authentication;

namespace DDDTemplate.Application.User
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<IServiceResponse> ForgotPasswordAsync(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<JwtMiddlewareResponse>> GetJwtUserbyIdAsync<T>(Guid? userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> ResetPasswordAsync(Guid? userId, string password, string passwordConfirm)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> SendActivationEmail(Guid? userId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<UserLoginResponse>> SignInAsync(string userEmail, string userPassword)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> SignUpAsync(UserRegisterRequest userDto)
        {
            throw new NotImplementedException();
        }

        public IServiceResponse<IsTokenValidResponse> ValidateToken<T>(string token)
        {
            throw new NotImplementedException();
        }
    }
}
