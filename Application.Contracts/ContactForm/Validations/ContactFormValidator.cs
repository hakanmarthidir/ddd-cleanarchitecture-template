using Application.Contracts.ContactForm.Request;
using FluentValidation;

namespace Application.Contracts.ContactForm.Validations
{
    public class ContactFormValidator : AbstractValidator<ContactFormCreateDto>
    {
        public ContactFormValidator()
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

            RuleFor(x => x.Topic)
               .NotEmpty().WithMessage("The Topic cannot be blank.")
               .Length(0, 255).WithMessage("The Topic cannot be more than 255 characters.");

            RuleFor(x => x.Message)
                 .NotEmpty().WithMessage("The Message cannot be blank.")
                 .Length(0, 1000).WithMessage("The Message cannot be more than 1000 characters.");

            RuleFor(x => x.Salutation)
                 .NotEmpty().WithMessage("The Salutation cannot be blank.");
        }

    }
}
