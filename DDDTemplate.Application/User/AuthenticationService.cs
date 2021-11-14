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

namespace DDDTemplate.Application.User
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IResponseService _responseService;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IUserRepository userRepository, IResponseService responseService, IMapper mapper, IHashService hashService, ITokenService tokenService)
        {
            this._userRepository = userRepository;
            this._responseService = responseService;
            this._mapper = mapper;
            this._hashService = hashService;
            this._tokenService = tokenService;
        }

        public async Task<IServiceResponse> SignUpAsync(UserRegisterDto userRegisterDto)
        {
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == userRegisterDto.Email).ConfigureAwait(false);
            Guard.Against.AlreadyExist(user, "Email address already exists for another account");

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

            return this._responseService.SuccessfulResponse("User created successfully");
        }

        public async Task<IServiceResponse<UserLoggedinDto>> SignInAsync(UserLoginDto userLoginDto)
        {
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            //TODO : count attempts
            if (!this._hashService.VerifyHashes(userLoginDto.Password, user.Password))
                throw new ArgumentException(message: $"Incorrect password.");

            var jwtUser = _mapper.Map<UserLoggedinDto>(user);
            Guard.Against.Null(jwtUser, nameof(jwtUser));

            jwtUser.Token = this._tokenService.GenerateToken(jwtUser.Id);

            return this._responseService.SuccessfulDataResponse(jwtUser);
        }

        public Task<IServiceResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IServiceResponse<JwtMiddlewareDto>> GetJwtUserbyIdAsync(UserIdDto userIdDto)
        {
            var user = await this._userRepository.FirstOrDefaultAsync(x => x.Id == userIdDto.Id && x.Status == Status.Active).ConfigureAwait(false);
            Guard.Against.Null(user, nameof(user), "User cannot be found.");

            var mappedUser = this._mapper.Map<JwtMiddlewareDto>(user);
            this.ValidateJwtUser(mappedUser);

            return this._responseService.SuccessfulDataResponse<JwtMiddlewareDto>(mappedUser);

        }

        public Task<IServiceResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse> SendActivationEmailAsync(UserIdDto userIdDto)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<TokenValidationDto>> ValidateTokenAsync(UserTokenDto userTokenDto)
        {
            throw new NotImplementedException();
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
