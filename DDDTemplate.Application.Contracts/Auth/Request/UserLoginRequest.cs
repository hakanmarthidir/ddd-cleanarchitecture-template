using System.ComponentModel.DataAnnotations;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Auth.Request
{
    public class UserLoginRequest : IDto
    {

        [Required]
        [EmailAddress]
        [StringLength(255, ErrorMessage = "Email format is wrong.", MinimumLength = 4)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Password format is wrong.", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
