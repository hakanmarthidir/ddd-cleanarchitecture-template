using System.ComponentModel.DataAnnotations;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Auth.Request
{
    public class ResetPasswordRequest : IDto
    {
        [Required]
        [StringLength(255, ErrorMessage = "Password format is wrong.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Password format is wrong.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
