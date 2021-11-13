using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Shared;
using FluentValidation;

namespace DDDTemplate.Application.Contracts.Auth.Validations
{
    public class UserTokenValidator : AbstractValidator<UserTokenDto>, IDataValidator
    {
        public UserTokenValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("The Token cannot be blank.");
        }

    }

}
