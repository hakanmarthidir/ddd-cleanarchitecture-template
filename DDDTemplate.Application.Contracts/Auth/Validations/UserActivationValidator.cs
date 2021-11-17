using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Shared;
using FluentValidation;

namespace DDDTemplate.Application.Contracts.Auth.Validations
{
    public class UserActivationValidator : AbstractValidator<UserActivationDto>, IDataValidator
    {
        public UserActivationValidator()
        {
            RuleFor(x => x.UserId)
               .NotNull().WithMessage("The UserId cannot be null.");
            RuleFor(x => x.ActivationCode)
               .NotEmpty().WithMessage("The ActivationCode cannot be blank.");

        }

    }
}
