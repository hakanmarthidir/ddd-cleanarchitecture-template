using System;
using Ardalis.GuardClauses;
using System.Linq;
using System.Collections.Generic;

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
    }
}
