using System;
using System.ComponentModel.DataAnnotations;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Auth.Request
{
    public class UserRegisterRequest : IDto
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255, ErrorMessage = "Email format is wrong.", MinimumLength = 4)]
        public string Email { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Password format is wrong.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Password format is wrong.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
