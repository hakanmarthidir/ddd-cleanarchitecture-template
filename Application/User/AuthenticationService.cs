using Application.Abstraction.Attributes;
using Application.Abstraction.Interfaces;
using Application.Abstraction.Response;
using Application.Abstraction.Response.Enums;
using Application.Abstraction.User;
using Application.Contracts.Auth.Request;
using Application.Contracts.Auth.Response;
using Application.Response;
using Ardalis.GuardClauses;
using AutoMapper;
using Core.Guard;
using Domain.Entities.UserAggregate.Enums;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.User
{
    [Performance]
    [Log(true)]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly ILogService<AuthenticationService> _logger;
        private readonly ITemplateService _templateService;
        private readonly IEmailService _mailGunService;


        public AuthenticationService(ILogService<AuthenticationService> logger, IUnitOfWork unitOfWork, IMapper mapper,
            IHashService hashService,
            ITokenService tokenService,
            ITemplateService templateService,
            IEmailService mailGunService
            )
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._hashService = hashService;
            this._tokenService = tokenService;
            this._logger = logger;
            this._templateService = templateService;
            this._mailGunService = mailGunService;
        }

        public async Task<IServiceResponse> SignUpAsync(UserRegisterDto userRegisterDto)
        {
            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == userRegisterDto.Email).ConfigureAwait(false);
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

            await this._unitOfWork.UserRepository.InsertAsync(newUser).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync().ConfigureAwait(false);

            var registeredUser = await GetUserByEmail(newUser.Email).ConfigureAwait(false);
            await this.SendActivationCodeEmailAsync(registeredUser, "Willkommen!").ConfigureAwait(false);

            return ServiceResponse.Success("User created successfully");
        }
        //[Performance]
        public async Task<IServiceResponse<UserLoggedinDto>> SignInAsync(UserLoginDto userLoginDto)
        {
            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            //TODO : count attempts
            var isVerifiedHash = await this._hashService.VerifyHashesAsync(userLoginDto.Password, user.Password).ConfigureAwait(false);
            Guard.Against.IsFalse(isVerifiedHash, $"Incorrect password.");

            var jwtUser = _mapper.Map<UserLoggedinDto>(user);
            Guard.Against.Null(jwtUser, nameof(jwtUser));

            jwtUser.Token = this._tokenService.GenerateToken(jwtUser.Id);
            Guard.Against.NullOrWhiteSpace(jwtUser.Token, nameof(jwtUser.Token), "Token could not be generated.");

            return ServiceResponse<UserLoggedinDto>.Success(jwtUser);
        }

        public async Task<IServiceResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            var token = this._tokenService.GenerateToken(user.Id);

            var forgotPasswordEmailBody = await this._templateService.GetForgotPasswordTemplateAsync(user.FirstName, token).ConfigureAwait(false);
            await this._mailGunService.SendEmailAsync(user.Email, "Passwort vergessen?", forgotPasswordEmailBody).ConfigureAwait(false);

            return ServiceResponse.Success();
        }

        public async Task<IServiceResponse<JwtMiddlewareDto>> GetJwtUserbyIdAsync(UserIdDto userIdDto)
        {
            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id && x.Status == Status.Active).ConfigureAwait(false);
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

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == userId && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.NullUser(userId.Value, user, "ResetPasswordAsync");

            user.SetPasswordAfterReset(newPassword);

            await this._unitOfWork.UserRepository.UpdateAsync(user).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync().ConfigureAwait(false);

            return ServiceResponse.Success();
        }

        public async Task<IServiceResponse> ReSendActivationEmailAsync(UserIdDto userIdDto)
        {
            Guard.Against.Null(userIdDto, nameof(userIdDto), "UserId cannot be found.");
            Guard.Against.NullOrEmpty(userIdDto.Id, nameof(userIdDto.Id), "UserId cannot be null.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id).ConfigureAwait(false);
            Guard.Against.NullUser(userIdDto.Id, user, "ReSendActivationEmailAsync");

            if (user.Activation.IsActivated == ActivationStatusEnum.Activated)
            {
                return ServiceResponse.Failure(ErrorCodes.INVALID_REQUEST, "User already activated.");
            }
            user.CreateActivationCode();
            await this._unitOfWork.UserRepository.UpdateAsync(user).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync().ConfigureAwait(false);

            await this.SendActivationCodeEmailAsync(user, "RE:Activation Code").ConfigureAwait(false);

            return ServiceResponse.Success();
        }

        public IServiceResponse<TokenValidationDto> ValidateToken(UserTokenDto userTokenDto)
        {
            Guard.Against.Null(userTokenDto, "Validate token", "Token cannot be found.");
            Guard.Against.NullOrWhiteSpace(userTokenDto.Token, nameof(userTokenDto.Token), "Token cannot be null.");

            var isValid = this._tokenService.ValidateCurrentToken(userTokenDto.Token);
            var response = new TokenValidationDto() { IsValid = isValid };

            return ServiceResponse<TokenValidationDto>.Success(response);
        }

        public async Task<IServiceResponse> ActivateAsync(UserActivationDto userActivationDto)
        {

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == userActivationDto.UserId
                                                                      && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.NullUser(userActivationDto.UserId.Value, user, "ActivateAsync");

            var canActivate = user.CanActivate(userActivationDto.ActivationCode);
            Guard.Against.IsFalse(canActivate, "User cannot be activated.");

            user.ActivateUser();

            await this._unitOfWork.UserRepository.UpdateAsync(user).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync().ConfigureAwait(false);
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
            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email && x.Status == Status.Active).ConfigureAwait(false);

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
