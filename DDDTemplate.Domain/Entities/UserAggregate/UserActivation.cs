using System;
using System.Collections.Generic;
using DDDTemplate.Domain.Entities.UserAggregate.Enums;
using DDDTemplate.Domain.Shared;

namespace DDDTemplate.Domain.Entities.UserAggregate
{
    public class UserActivation : ValueObject
    {
        public virtual ActivationStatus IsActivated { get; set; }
        public virtual string ActivationCode { get; set; }
        public virtual DateTimeOffset? ActivationDate { get; set; }
        public UserActivation()
        {
            IsActivated = ActivationStatus.NotActivated;
            ActivationCode = Guid.NewGuid().ToString();            
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ActivationCode;
            yield return IsActivated;          
        }
    }
}
