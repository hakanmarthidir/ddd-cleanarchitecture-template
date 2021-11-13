using DDDTemplate.Application.Contracts.Auth.Request;
using DDDTemplate.Application.Contracts.Shared;
using FluentValidation;

namespace DDDTemplate.Application.Contracts.Auth.Validations
{
    public class UserIdValidator : AbstractValidator<UserIdDto>, IDataValidator
    {
        public UserIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("The Id cannot be blank.");
        }

    }

}
