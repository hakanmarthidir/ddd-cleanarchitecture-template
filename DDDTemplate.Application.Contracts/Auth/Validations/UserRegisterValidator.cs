using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Shared;
using FluentValidation;

namespace DDDTemplate.Application.Contracts.Auth.Validations
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>, IDataValidator
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("The FirstName cannot be blank.")
                .Length(0, 255).WithMessage("The FirstName cannot be more than 255 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("The LastName cannot be blank.")
                .Length(0, 255).WithMessage("The LastName cannot be more than 255 characters.");

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("The Email cannot be blank.")
               .Length(0, 255).WithMessage("The Email cannot be more than 255 characters.").
               EmailAddress().WithMessage("Email is not valid.");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("The Password cannot be blank.")
               .Length(0, 255).WithMessage("The Password cannot be more than 255 characters.");

            RuleFor(x => x.PasswordConfirm)
                 .NotEmpty().WithMessage("The PasswordConfirm cannot be blank.")
                 .Length(0, 255).WithMessage("The PasswordConfirm cannot be more than 255 characters.");

        }

    }

}
