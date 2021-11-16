using System;
using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Auth.Response;
using DDDTemplate.Infrastructure.Response.Base;
using DDDTemplate.Domain.AggregatesModel.UserAggregate;
using DDDTemplate.Infrastructure.Response;
using DDDTemplate.Core.Guard;
using AutoMapper;
using Ardalis.GuardClauses;
using DDDTemplate.Infrastructure.Security.Hash;
using DDDTemplate.Infrastructure.Security.Token;
using DDDTemplate.Application.Abstraction.User;
using DDDTemplate.Domain.Enums;
using Microsoft.Extensions.Logging;
using DDDTemplate.Domain.AggregatesModel.UserAggregate.Enums;
using DDDTemplate.Infrastructure.Response.Enums;

namespace DDDTemplate.Application.User
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IResponseService _responseService;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ILogger<AuthenticationService> logger, IUserRepository userRepository, IResponseService responseService, IMapper mapper, IHashService hashService, ITokenService tokenService)
        {
            this._userRepository = userRepository;
            this._responseService = responseService;
            this._mapper = mapper;
            this._hashService = hashService;
            this._tokenService = tokenService;
            this._logger = logger;
        }

        public async Task<IServiceResponse> SignUpAsync(UserRegisterDto userRegisterDto)
        {
            this._logger.LogInformation($"{userRegisterDto.Email} - Signing Up ");

            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == userRegisterDto.Email).ConfigureAwait(false);
            Guard.Against.AlreadyExist(user, $"{userRegisterDto.Email} - Email address already exists.");

            var hashedPassword = this._hashService.GetHashedString(userRegisterDto.Password);
            Guard.Against.NullOrWhiteSpace(hashedPassword, nameof(userRegisterDto.Password), "Hashing problem occured.");

            var newUser = new Domain.AggregatesModel.UserAggregate.User()
            {
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Email = userRegisterDto.Email,
                Password = hashedPassword
            };

            newUser.SetCreationValues();
            newUser.CreateActivationCode();

            await this._userRepository.InsertAsync(newUser).ConfigureAwait(false);
            //TODO: Send Activation Email here using classic way or queue.

            this._logger.LogInformation($"{userRegisterDto.Email} - Account was created successfully.");
            return this._responseService.SuccessfulResponse("User created successfully");
        }

        public async Task<IServiceResponse<UserLoggedinDto>> SignInAsync(UserLoginDto userLoginDto)
        {
            this._logger.LogInformation($"{userLoginDto.Email} - Loggining to system");
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            //TODO : count attempts
            if (!this._hashService.VerifyHashes(userLoginDto.Password, user.Password))
                throw new ArgumentException(message: $"Incorrect password.");

            var jwtUser = _mapper.Map<UserLoggedinDto>(user);
            Guard.Against.Null(jwtUser, nameof(jwtUser));

            jwtUser.Token = this._tokenService.GenerateToken(jwtUser.Id);
            Guard.Against.NullOrWhiteSpace(jwtUser.Token, nameof(jwtUser.Token), "Token could not be generated.");

            this._logger.LogInformation($"{userLoginDto.Email} - Logged in successfully.");
            return this._responseService.SuccessfulDataResponse(jwtUser);
        }

        public async Task<IServiceResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {

            this._logger.LogInformation($"{forgotPasswordDto.Email} - forgot password operation was started.");
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            var token = this._tokenService.GenerateToken(user.Id);

            //TODO: Send Email For Reset
            //await this._emailService.SendForgotPasswordAsync(user.FirstName, passToken, user.Email).ConfigureAwait(false);

            this._logger.LogInformation($"{forgotPasswordDto.Email} - created a forgot password request.");
            return this._responseService.SuccessfulResponse();
        }

        public async Task<IServiceResponse<JwtMiddlewareDto>> GetJwtUserbyIdAsync(UserIdDto userIdDto)
        {
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            var mappedUser = this._mapper.Map<JwtMiddlewareDto>(user);
            this.ValidateJwtUser(mappedUser);

            return this._responseService.SuccessfulDataResponse<JwtMiddlewareDto>(mappedUser);

        }

        public async Task<IServiceResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var newPassword = this._hashService.GetHashedString(resetPasswordDto.Password);
            var userId = this._tokenService.GetUserIdByToken(resetPasswordDto.Token);
            Guard.Against.NullOrEmpty(userId, nameof(userId), $"{resetPasswordDto.Token} - Invalid token.");

            this._logger.LogInformation($"{userId} - password reset operation was started.");
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Id == userId && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            user.SetPasswordAfterReset(newPassword);

            await this._userRepository.UpdateAsync(user).ConfigureAwait(false);

            this._logger.LogInformation($"{userId} - password successfully updated.");
            return this._responseService.SuccessfulResponse();
        }

        public async Task<IServiceResponse> ReSendActivationEmailAsync(UserIdDto userIdDto)
        {
            Guard.Against.Null(userIdDto, nameof(userIdDto), "UserId cannot be found.");
            Guard.Against.NullOrEmpty(userIdDto.Id, nameof(userIdDto.Id), "UserId cannot be null.");

            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            if (user.IsActivated == ActivationStatus.Activated)
            {
                this._logger.LogInformation($"{userIdDto.Id} - already activated.");
                return this._responseService.FailedResponse(ErrorCodes.INVALID_REQUEST, "User already activated.");
            }
            user.CreateActivationCode();
            await this._userRepository.UpdateAsync(user).ConfigureAwait(false);

            //TODO : Send Activation Email to User
            //await this._emailService.SendEmailAfterSignupAsync(user.Id.ToString(), user.Email, user.FirstName, user.LastName, user.ActivationCode);
            this._logger.LogInformation($"{user.Email} - requested a new activation code.");

            return this._responseService.SuccessfulResponse();
        }

        public IServiceResponse<TokenValidationDto> ValidateToken(UserTokenDto userTokenDto)
        {
            Guard.Against.Null(userTokenDto, "Validate token", "Token cannot be found.");
            Guard.Against.NullOrWhiteSpace(userTokenDto.Token, nameof(userTokenDto.Token), "Token cannot be null.");

            this._logger.LogInformation($"{userTokenDto.Token} - token validation operation was started.");
            var isValid = this._tokenService.ValidateCurrentToken(userTokenDto.Token);
            var response = new TokenValidationDto() { IsValid = isValid };

            this._logger.LogInformation($"{userTokenDto.Token} validation result is  {isValid}");
            return this._responseService.SuccessfulDataResponse<TokenValidationDto>(response);
        }

        private void ValidateJwtUser(JwtMiddlewareDto jwtMiddlewareDto)
        {
            Guard.Against.Null(jwtMiddlewareDto, nameof(jwtMiddlewareDto));
            Guard.Against.NullOrWhiteSpace(jwtMiddlewareDto.Email, nameof(jwtMiddlewareDto.Email));
            Guard.Against.NullOrWhiteSpace(jwtMiddlewareDto.FirstName, nameof(jwtMiddlewareDto.Email));
            Guard.Against.NullOrWhiteSpace(jwtMiddlewareDto.LastName, nameof(jwtMiddlewareDto.Email));
            Guard.Against.NullOrEmpty(jwtMiddlewareDto.Id, nameof(jwtMiddlewareDto.Id));
        }
    }
}
