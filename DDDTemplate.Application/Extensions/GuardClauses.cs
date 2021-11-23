using System;
using Ardalis.GuardClauses;
using System.Linq;
using System.Collections.Generic;
using DDDTemplate.Domain.AggregatesModel.UserAggregate;
using DDDTemplate.Domain.Entities.UserAggregate.Exceptions;

namespace DDDTemplate.Core.Guard
{
    public static class GuardClausesExtensions
    {

        public static void Any<T>(this IGuardClause guardClause, ICollection<T> collection, string message)
        {
            if (collection.Any())
                throw new ArgumentException(message);
        }

        public static void AlreadyExist<T>(this IGuardClause guardClause, T input, string message)
        {
            if (input != null)
                throw new ArgumentException(message);
        }

        public static void IsFalse(this IGuardClause guardClause, bool input, string message)
        {
            if (input == false)
                throw new ArgumentException(message);
        }

        public static void NullUser(this IGuardClause guardClause, Guid userId, User user, string functionName)
        {
            if (user == null)
                throw new UserNotFoundException(userId, functionName);
        }
    }
}
