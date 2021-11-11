using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Auth.Response;
using DDDTemplate.Infrastructure.Response.Base;

namespace DDDTemplate.Application.Abstraction.Authentication
{
    public interface IAuthenticationService
    {
        Task<IServiceResponse> SignUpAsync(UserRegisterRequest userDto);
        Task<IServiceResponse<UserLoginResponse>> SignInAsync(string userEmail, string userPassword);
        Task<IServiceResponse<JwtMiddlewareResponse>> GetJwtUserbyIdAsync(Guid userId);
        IServiceResponse<IsTokenValidResponse> ValidateToken(string token);
        Task<IServiceResponse> ForgotPasswordAsync(string userEmail);
        Task<IServiceResponse> ResetPasswordAsync(Guid userId, string password, string passwordConfirm);
        Task<IServiceResponse> SendActivationEmail(Guid userId);
    }
}
