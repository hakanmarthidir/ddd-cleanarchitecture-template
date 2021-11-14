using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Auth.Response;
using DDDTemplate.Infrastructure.Response.Base;

namespace DDDTemplate.Application.Abstraction.User
{
    public interface IAuthenticationService
    {
        Task<IServiceResponse> SignUpAsync(UserRegisterDto userDto);
        Task<IServiceResponse<UserLoggedinDto>> SignInAsync(UserLoginDto userLoginDto);
        Task<IServiceResponse<JwtMiddlewareDto>> GetJwtUserbyIdAsync(UserIdDto userIdDto);
        Task<IServiceResponse<TokenValidationDto>> ValidateTokenAsync(UserTokenDto userTokenDto);
        Task<IServiceResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<IServiceResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<IServiceResponse> SendActivationEmailAsync(UserIdDto userIdDto);
    }
}
