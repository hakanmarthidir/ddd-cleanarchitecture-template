using Ardalis.SmartEnum;

namespace DDDTemplate.Domain.Entities.UserAggregate.Enums
{
    public sealed class UserTypeEnum : SmartEnum<UserTypeEnum>
    {
        public static readonly UserTypeEnum Administrator = new UserTypeEnum(nameof(Administrator), 0);
        public static readonly UserTypeEnum User = new UserTypeEnum(nameof(User), 1);
        public UserTypeEnum(string name, int value) : base(name, value)
        {
        }
    }
}
