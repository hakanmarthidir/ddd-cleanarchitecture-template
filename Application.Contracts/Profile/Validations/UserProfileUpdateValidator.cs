using Application.Contracts.Profile.Request;
using FluentValidation;

namespace Application.Contracts.Profile.Validations
{
    public class UserProfileUpdateValidator : AbstractValidator<UserProfileUpdateDto>
    {
        public UserProfileUpdateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("The FirstName cannot be blank.")
                .Length(0, 255).WithMessage("The FirstName cannot be more than 255 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("The LastName cannot be blank.")
                .Length(0, 255).WithMessage("The LastName cannot be more than 255 characters.");

        }

    }
}
