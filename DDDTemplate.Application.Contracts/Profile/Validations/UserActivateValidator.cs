using DDDTemplate.Application.Contracts.Profile.Request;
using DDDTemplate.Application.Contracts.Shared;
using FluentValidation;

namespace DDDTemplate.Application.Contracts.Profile.Validations
{
    public class UserActivateValidator : AbstractValidator<UserActivateDto>, IDataValidator
    {
        public UserActivateValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("The UserId cannot be blank.");

            RuleFor(x => x.ActivationCode)
                .NotEmpty().WithMessage("The ActivationCode cannot be blank.")
                .Length(0, 255).WithMessage("The ActivationCode cannot be more than 255 characters.");

        }

    }
}
