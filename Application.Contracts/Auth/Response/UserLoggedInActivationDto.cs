using Domain.Entities.UserAggregate.Enums;

namespace Application.Contracts.Auth.Response
{
    public class UserLoggedinActivationDto
    {
        public virtual ActivationStatusEnum IsActivated { get; set; }
        public virtual DateTimeOffset? ActivationDate { get; set; }

    }
}
