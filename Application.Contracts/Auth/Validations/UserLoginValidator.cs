﻿using Application.Contracts.Auth.Request;
using FluentValidation;

namespace Application.Contracts.Auth.Validations
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("The Email cannot be blank.")
               .Length(0, 255).WithMessage("The Email cannot be more than 255 characters.").
               EmailAddress().WithMessage("Email is not valid.");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("The Password cannot be blank.")
               .Length(0, 255).WithMessage("The Password cannot be more than 255 characters.");

        }

    }
}
