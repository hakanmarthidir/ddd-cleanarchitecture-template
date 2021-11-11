using System.ComponentModel.DataAnnotations;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Profile.Request
{
    public class UserProfileUpdateRequest : IDto
    {
        [Required]
        public virtual string FirstName { get; set; }
        [Required]
        public virtual string LastName { get; set; }

    }

}
