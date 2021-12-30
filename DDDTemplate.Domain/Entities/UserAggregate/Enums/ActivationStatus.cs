using Ardalis.SmartEnum;

namespace DDDTemplate.Domain.Entities.UserAggregate.Enums
{
    public sealed class ActivationStatusEnum : SmartEnum<ActivationStatusEnum>
    {
        public static readonly ActivationStatusEnum NotActivated = new ActivationStatusEnum(nameof(NotActivated), 0);
        public static readonly  ActivationStatusEnum Activated = new ActivationStatusEnum(nameof(Activated), 1);
        public ActivationStatusEnum(string name, int value) : base(name, value)
        {
        }
    }

}
