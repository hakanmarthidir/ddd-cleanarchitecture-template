using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Shared;
using FluentValidation;

namespace DDDTemplate.Application.Contracts.Auth.Validations
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>, IDataValidator
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Token)
               .NotEmpty().WithMessage("The Token cannot be blank.");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("The Password cannot be blank.")
               .Length(0, 255).WithMessage("The Password cannot be more than 255 characters.");

            RuleFor(x => x.PasswordConfirm)
                 .NotEmpty().WithMessage("The PasswordConfirm cannot be blank.")
                 .Length(0, 255).WithMessage("The PasswordConfirm cannot be more than 255 characters.");

        }

    }

}
