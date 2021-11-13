﻿using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Shared;
using FluentValidation;

namespace DDDTemplate.Application.Contracts.Auth.Validations
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>, IDataValidator
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("The Email cannot be blank.")
               .Length(0, 255).WithMessage("The Email cannot be more than 255 characters.").
               EmailAddress().WithMessage("Email is not valid.");

        }

    }

}