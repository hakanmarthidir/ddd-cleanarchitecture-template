using System.ComponentModel.DataAnnotations;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Auth.Request
{
    public class ForgotPasswordRequest : IDto
    {
        [Required]
        [EmailAddress]
        [StringLength(255, ErrorMessage = "Email format is wrong.", MinimumLength = 4)]
        public string Email { get; set; }
    }
}
