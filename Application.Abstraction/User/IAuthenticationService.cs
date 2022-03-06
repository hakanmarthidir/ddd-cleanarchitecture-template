using System;
using System.Threading.Tasks;
using Application.Abstraction.Response;
using Application.Contracts.Auth.Request;
using Application.Contracts.Auth.Response;

namespace Application.Abstraction.User
{
    public interface IAuthenticationService
    {
        Task<IServiceResponse> SignUpAsync(UserRegisterDto userDto);
        Task<IServiceResponse<UserLoggedinDto>> SignInAsync(UserLoginDto userLoginDto);
        Task<IServiceResponse<JwtMiddlewareDto>> GetJwtUserbyIdAsync(UserIdDto userIdDto);
        IServiceResponse<TokenValidationDto> ValidateToken(UserTokenDto userTokenDto);
        Task<IServiceResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<IServiceResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<IServiceResponse> ReSendActivationEmailAsync(UserIdDto userIdDto);
        Task<IServiceResponse> ActivateAsync(UserActivationDto userActivationDto);
        Task<IServiceResponse<List<GetRegisteredUserEmailDto>>> GetRegisteredUserList(int page, int pageSize);
    }
}
