using System;
namespace DDDTemplate.Domain.Entities.UserAggregate.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid userId, string functionName) : base($"User can not be found with Id {userId} within the {functionName}")
        {
        }
    }
}
