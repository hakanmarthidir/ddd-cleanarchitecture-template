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
using Domain.Entities.UserAggregate.Specifications;
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
            Guard.Against.Null(userRegisterDto, nameof(userRegisterDto), "User could not be null to register.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == userRegisterDto.Email).ConfigureAwait(false);
            Guard.Against.AlreadyExist(user, $"{userRegisterDto.Email} - Email address already exists.");

            var hashedPassword = await this._hashService.GetHashedStringAsync(userRegisterDto.Password).ConfigureAwait(false);

            var newUser = Domain.Entities.UserAggregate.User.CreateUser
            (
                userRegisterDto.FirstName,
                userRegisterDto.LastName,
                userRegisterDto.Email,
                hashedPassword
            );            

            await this._unitOfWork.UserRepository.InsertAsync(newUser).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync().ConfigureAwait(false);

            return ServiceResponse.Success("User was created successfully.");
        }

        public async Task<IServiceResponse<UserLoggedinDto>> SignInAsync(UserLoginDto userLoginDto)
        {
            Guard.Against.Null(userLoginDto, nameof(userLoginDto), "User could not be null to signin.");
            Guard.Against.NullOrWhiteSpace(userLoginDto.Email, nameof(userLoginDto.Email), "Email could not be null to signin.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User could not be found.");

            var isVerifiedHash = await this._hashService.VerifyHashesAsync(userLoginDto.Password, user.Password).ConfigureAwait(false);
            Guard.Against.IsFalse(isVerifiedHash, $"A verification problem occured.");


            var jwtUser = Guard.Against.Null(_mapper.Map<UserLoggedinDto>(user), nameof(user));

            jwtUser.Token = this._tokenService.GenerateToken(jwtUser.Id);
            Guard.Against.NullOrWhiteSpace(jwtUser.Token, nameof(jwtUser.Token), "Token could not be generated.");

            return ServiceResponse<UserLoggedinDto>.Success(jwtUser);
        }

        public async Task<IServiceResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            Guard.Against.Null(forgotPasswordDto, nameof(forgotPasswordDto), "User data could not be null.");
            Guard.Against.NullOrWhiteSpace(forgotPasswordDto.Email, nameof(forgotPasswordDto.Email), "Email could not be null.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User could not be found.");

            var token = this._tokenService.GenerateToken(user.Id);
            Guard.Against.NullOrWhiteSpace(token, nameof(token), "Token could not be generated.");

            var forgotPasswordEmailBody = await this._templateService.GetForgotPasswordTemplateAsync(user.FirstName, token).ConfigureAwait(false);
            await this._mailGunService.SendEmailAsync(user.Email, "Forgot password?", forgotPasswordEmailBody).ConfigureAwait(false);

            return ServiceResponse.Success();
        }

        public async Task<IServiceResponse<JwtMiddlewareDto>> GetJwtUserbyIdAsync(UserIdDto userIdDto)
        {
            Guard.Against.Null(userIdDto, nameof(userIdDto), "User data could not be null.");
            Guard.Against.NullOrEmpty(userIdDto.Id, nameof(userIdDto.Id), "UserId could not be null.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User could not be found.");

            var mappedUser = this._mapper.Map<JwtMiddlewareDto>(user);
            this.ValidateJwtUser(mappedUser);

            return ServiceResponse<JwtMiddlewareDto>.Success(mappedUser);
        }

        public async Task<IServiceResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            Guard.Against.Null(resetPasswordDto, nameof(resetPasswordDto), "User data could not be null to reset password.");
            Guard.Against.NullOrEmpty(resetPasswordDto.Token, nameof(resetPasswordDto.Token), $"Token could not be null.");

            var newPassword = await this._hashService.GetHashedStringAsync(resetPasswordDto.Password).ConfigureAwait(false);
            Guard.Against.NullOrEmpty(newPassword, nameof(newPassword), $"NewPassword could not be created.");

            var userId = this._tokenService.GetUserIdByToken(resetPasswordDto.Token);
            Guard.Against.NullOrEmpty(userId, nameof(userId), $"{resetPasswordDto.Token} - Invalid token.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == userId && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User could not be found.");

            user.SetPasswordAfterReset(newPassword);

            await this._unitOfWork.UserRepository.UpdateAsync(user).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync().ConfigureAwait(false);

            return ServiceResponse.Success();
        }

        public async Task<IServiceResponse> ReSendActivationEmailAsync(UserIdDto userIdDto)
        {
            Guard.Against.Null(userIdDto, nameof(userIdDto), "User data could not be null.");
            Guard.Against.NullOrEmpty(userIdDto.Id, nameof(userIdDto.Id), "UserId could not be null.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User could not be found.");

            if (user.Activation.IsActivated == ActivationStatusEnum.Activated)            
                return ServiceResponse.Failure(ErrorCodes.INVALID_REQUEST, "User already activated.");
            
            user.CreateNewActivationCode();            

            await this._unitOfWork.UserRepository.UpdateAsync(user).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync().ConfigureAwait(false);

            return ServiceResponse.Success();
        }

        public IServiceResponse<TokenValidationDto> ValidateToken(UserTokenDto userTokenDto)
        {
            Guard.Against.Null(userTokenDto, nameof(userTokenDto), "Token could not be null.");

            var isValid = this._tokenService.ValidateCurrentToken(userTokenDto.Token);
            var response = new TokenValidationDto() { IsValid = isValid };

            return ServiceResponse<TokenValidationDto>.Success(response);
        }

        public async Task<IServiceResponse> ActivateAsync(UserActivationDto userActivationDto)
        {
            Guard.Against.Null(userActivationDto, nameof(userActivationDto), "User could not be null.");
            Guard.Against.NullOrEmpty(userActivationDto.UserId, nameof(userActivationDto.UserId), "UserId could not be null.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Id == userActivationDto.UserId
                                                                      && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User could not be found.");            

            var canActivate = user.CanActivate(userActivationDto.ActivationCode);
            Guard.Against.IsFalse(canActivate, "User could not be activated.");

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

        public async Task<IServiceResponse<List<GetRegisteredUserEmailDto>>> GetRegisteredUserList(int page, int pageSize)
        {
            var result = await this._unitOfWork.UserRepository.FindAsync(new GetRegisteredUserSpec( Status.Active, page, pageSize)).ConfigureAwait(false);

            var userList = this._mapper.Map<List<GetRegisteredUserEmailDto>>(result);
            Guard.Against.Null(userList, nameof(userList));

            return ServiceResponse<List<GetRegisteredUserEmailDto>>.Success(userList);
        }

    }
}
