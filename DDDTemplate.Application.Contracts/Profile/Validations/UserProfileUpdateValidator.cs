using System;
using DDDTemplate.Application.Contracts.Profile.Request;
using DDDTemplate.Application.Contracts.Shared;
using FluentValidation;

namespace DDDTemplate.Application.Contracts.Profile.Validations
{
    public class UserProfileUpdateValidator : AbstractValidator<UserProfileUpdateDto>, IDataValidator
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
