using System.Threading.Tasks;
using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Auth.Response;
using DDDTemplate.Domain.Entities.UserAggregate;
using DDDTemplate.Core.Guard;
using AutoMapper;
using Ardalis.GuardClauses;
using DDDTemplate.Application.Abstraction.User;
using DDDTemplate.Domain.Enums;
using Microsoft.Extensions.Logging;
using DDDTemplate.Domain.Entities.UserAggregate.Enums;
using DDDTemplate.Application.Abstraction.Response;
using DDDTemplate.Application.Response;
using DDDTemplate.Application.Abstraction.Response.Enums;
using DDDTemplate.Application.Abstraction.Interfaces;

namespace DDDTemplate.Application.User
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly ILogService<AuthenticationService> _logger;
        private readonly ITemplateService _templateService;
        private readonly IEmailService _mailGunService;


        public AuthenticationService(ILogService<AuthenticationService> logger, IUserRepository userRepository, IMapper mapper,
            IHashService hashService,
            ITokenService tokenService,
            ITemplateService templateService,
            IEmailService mailGunService
            )
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._hashService = hashService;
            this._tokenService = tokenService;
            this._logger = logger;
            this._templateService = templateService;
            this._mailGunService = mailGunService;
        }

        public async Task<IServiceResponse> SignUpAsync(UserRegisterDto userRegisterDto)
        {
            this._logger.LogInformation($"{userRegisterDto.Email} - Signing Up ");

            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == userRegisterDto.Email).ConfigureAwait(false);
            Guard.Against.AlreadyExist(user, $"{userRegisterDto.Email} - Email address already exists.");

            var hashedPassword = await this._hashService.GetHashedStringAsync(userRegisterDto.Password).ConfigureAwait(false);
            Guard.Against.NullOrWhiteSpace(hashedPassword, nameof(userRegisterDto.Password), "Hashing problem occured.");

            var newUser = Domain.Entities.UserAggregate.User.CreateUser
            (
                userRegisterDto.FirstName,
                userRegisterDto.LastName,
                userRegisterDto.Email,
                hashedPassword
            );

            await this._userRepository.InsertAsync(newUser).ConfigureAwait(false);

            var registeredUser = await GetUserByEmail(newUser.Email).ConfigureAwait(false);
            await this.SendActivationCodeEmailAsync(registeredUser, "Willkommen!").ConfigureAwait(false);

            this._logger.LogInformation($"{userRegisterDto.Email} - Account was created successfully.");
            return ServiceResponse.Success("User created successfully");
        }

        public async Task<IServiceResponse<UserLoggedinDto>> SignInAsync(UserLoginDto userLoginDto)
        {
            this._logger.LogInformation($"{userLoginDto.Email} - Loggining to system");
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            //TODO : count attempts
            var isVerifiedHash = await this._hashService.VerifyHashesAsync(userLoginDto.Password, user.Password).ConfigureAwait(false);
            Guard.Against.IsFalse(isVerifiedHash, $"Incorrect password.");

            var jwtUser = _mapper.Map<UserLoggedinDto>(user);
            Guard.Against.Null(jwtUser, nameof(jwtUser));

            jwtUser.Token = this._tokenService.GenerateToken(jwtUser.Id);
            Guard.Against.NullOrWhiteSpace(jwtUser.Token, nameof(jwtUser.Token), "Token could not be generated.");

            this._logger.LogInformation($"{userLoginDto.Email} - Logged in successfully.");
            return ServiceResponse<UserLoggedinDto>.Success(jwtUser);
        }

        public async Task<IServiceResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {

            this._logger.LogInformation($"{forgotPasswordDto.Email} - forgot password operation was started.");
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            var token = this._tokenService.GenerateToken(user.Id);

            var forgotPasswordEmailBody = await this._templateService.GetForgotPasswordTemplateAsync(user.FirstName, token).ConfigureAwait(false);
            await this._mailGunService.SendEmailAsync(user.Email, "Passwort vergessen?", forgotPasswordEmailBody).ConfigureAwait(false);

            this._logger.LogInformation($"{forgotPasswordDto.Email} - created a forgot password request.");
            return ServiceResponse.Success();
        }

        public async Task<IServiceResponse<JwtMiddlewareDto>> GetJwtUserbyIdAsync(UserIdDto userIdDto)
        {
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.NullUser(userIdDto.Id, user, "GetJwtUserbyIdAsync");

            var mappedUser = this._mapper.Map<JwtMiddlewareDto>(user);
            this.ValidateJwtUser(mappedUser);

            return ServiceResponse<JwtMiddlewareDto>.Success(mappedUser);
        }

        public async Task<IServiceResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var newPassword = await this._hashService.GetHashedStringAsync(resetPasswordDto.Password).ConfigureAwait(false);
            Guard.Against.NullOrEmpty(newPassword, nameof(newPassword), $"NewPassword could not be created.");

            var userId = this._tokenService.GetUserIdByToken(resetPasswordDto.Token);
            Guard.Against.NullOrEmpty(userId, nameof(userId), $"{resetPasswordDto.Token} - Invalid token.");

            this._logger.LogInformation($"{userId} - password reset operation was started.");
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Id == userId && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.NullUser(userId.Value, user, "ResetPasswordAsync");

            user.SetPasswordAfterReset(newPassword);

            await this._userRepository.UpdateAsync(user).ConfigureAwait(false);

            this._logger.LogInformation($"{userId} - password successfully updated.");
            return ServiceResponse.Success();
        }

        public async Task<IServiceResponse> ReSendActivationEmailAsync(UserIdDto userIdDto)
        {
            Guard.Against.Null(userIdDto, nameof(userIdDto), "UserId cannot be found.");
            Guard.Against.NullOrEmpty(userIdDto.Id, nameof(userIdDto.Id), "UserId cannot be null.");

            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id).ConfigureAwait(false);
            Guard.Against.NullUser(userIdDto.Id, user, "ReSendActivationEmailAsync");

            if (user.Activation.IsActivated == ActivationStatusEnum.Activated)
            {
                this._logger.LogInformation($"{userIdDto.Id} - already activated.");
                return ServiceResponse.Failure(ErrorCodes.INVALID_REQUEST, "User already activated.");
            }
            user.CreateActivationCode();
            await this._userRepository.UpdateAsync(user).ConfigureAwait(false);

            await this.SendActivationCodeEmailAsync(user, "RE:Activation Code").ConfigureAwait(false);

            this._logger.LogInformation($"{user.Email} - requested a new activation code.");
            return ServiceResponse.Success();
        }

        public IServiceResponse<TokenValidationDto> ValidateToken(UserTokenDto userTokenDto)
        {
            Guard.Against.Null(userTokenDto, "Validate token", "Token cannot be found.");
            Guard.Against.NullOrWhiteSpace(userTokenDto.Token, nameof(userTokenDto.Token), "Token cannot be null.");

            this._logger.LogInformation($"{userTokenDto.Token} - token validation operation was started.");
            var isValid = this._tokenService.ValidateCurrentToken(userTokenDto.Token);
            var response = new TokenValidationDto() { IsValid = isValid };

            this._logger.LogInformation($"{userTokenDto.Token} validation result is  {isValid}");
            return ServiceResponse<TokenValidationDto>.Success(response);
        }

        public async Task<IServiceResponse> ActivateAsync(UserActivationDto userActivationDto)
        {
            this._logger.LogInformation($"{userActivationDto.UserId} - activation operation was started.");

            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Id == userActivationDto.UserId
                                                                      && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.NullUser(userActivationDto.UserId.Value, user, "ActivateAsync");

            var canActivate = user.CanActivate(userActivationDto.ActivationCode);
            Guard.Against.IsFalse(canActivate, "User cannot be activated.");

            user.ActivateUser();

            await this._userRepository.UpdateAsync(user).ConfigureAwait(false);
            this._logger.LogInformation($"{userActivationDto.UserId} - successfully activated.");

            return ServiceResponse.Success();
        }

        private void ValidateJwtUser(JwtMiddlewareDto jwtMiddlewareDto)
        {
            Guard.Against.Null(jwtMiddlewareDto, nameof(jwtMiddlewareDto));
            Guard.Against.NullOrWhiteSpace(jwtMiddlewareDto.Email, nameof(jwtMiddlewareDto.Email));
            Guard.Against.NullOrWhiteSpace(jwtMiddlewareDto.FirstName, nameof(jwtMiddlewareDto.Email));
            Guard.Against.NullOrWhiteSpace(jwtMiddlewareDto.LastName, nameof(jwtMiddlewareDto.Email));
            Guard.Against.NullOrEmpty(jwtMiddlewareDto.Id, nameof(jwtMiddlewareDto.Id));
        }

        private async Task<Domain.Entities.UserAggregate.User> GetUserByEmail(string email)
        {
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == email && x.Status == Status.Active).ConfigureAwait(false);

            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            return user;
        }

        private async Task SendActivationCodeEmailAsync(Domain.Entities.UserAggregate.User registeredUser, string emailTitle)
        {
            var activationEmailBody = await this._templateService.GetActivationTemplateAsync(
                registeredUser.Id.ToString(),
                registeredUser.FirstName,
                registeredUser.LastName,
                registeredUser.Activation.ActivationCode).ConfigureAwait(false);

            await this._mailGunService.SendEmailAsync(registeredUser.Email, emailTitle, activationEmailBody).ConfigureAwait(false);
        }
    }
}
