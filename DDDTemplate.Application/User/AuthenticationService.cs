using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Auth.Response;
using DDDTemplate.Infrastructure.Response.Base;
using DDDTemplate.Application.Abstraction.Authentication;
using DDDTemplate.Domain.AggregatesModel.UserAggregate;
using DDDTemplate.Infrastructure.Response;

namespace DDDTemplate.Application.User
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IResponseService _responseService;

        public AuthenticationService(IUserRepository userRepository, IResponseService responseService)
        {
            this._userRepository = userRepository;
            this._responseService = responseService;
        }

        public async Task<IServiceResponse> SignUpAsync(UserRegisterDto userRegisterDto)
        {
            var newUser = new Domain.AggregatesModel.UserAggregate.User()
            {
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Email = userRegisterDto.Email,
                Password = userRegisterDto.Password,
                IsActivated = Domain.AggregatesModel.UserAggregate.Enums.ActivationStatus.NotActivated,
                Status = Domain.SeedWork.Status.Active,
                CreatedDate = DateTimeOffset.UtcNow,
                UserType = Domain.AggregatesModel.UserAggregate.Enums.UserType.User,
                ActivationCode = Guid.NewGuid().ToString()
            };

            await this._userRepository.InsertAsync(newUser);

            return this._responseService.SuccessfulResponse("User created successfully");
        }

        public Task<IServiceResponse<UserLoggedinDto>> SignInAsync(UserLoginDto userLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<JwtMiddlewareDto>> GetJwtUserbyIdAsync(UserIdDto userIdDto)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> SendActivationEmailAsync(UserIdDto userIdDto)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<IsTokenValidDto>> ValidateTokenAsync(UserTokenDto userTokenDto)
        {
            throw new NotImplementedException();
        }
    }
}
