using System;
using System.IO;

namespace DDDTemplate.Core.Guard
{
    public static class GuardClauses
    {

        public static void ArgumentNotNullOrWhitespace(string argumentValue, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argumentValue))
                throw new ArgumentNullException(paramName: argumentName, $"{argumentName} could not be null or whitespace.");
        }

        public static void ArgumentNotNullOrWhitespace(string argumentValue, string argumentName, string message)
        {
            if (string.IsNullOrWhiteSpace(argumentValue))
                throw new ArgumentNullException(paramName: argumentName, message: message);
        }

        public static void ArgumentNotNull<T>(T argumentValue, string argumentName) where T : class
        {
            if (argumentValue == null)
                throw new ArgumentNullException(paramName: argumentName, $"{argumentName} could not be null.");
        }

        public static void ArgumentNotNull<T>(T argumentValue, string argumentName, string message) where T : class
        {
            if (argumentValue == null)
                throw new ArgumentNullException(paramName: argumentName, message: message);
        }

        public static void FileNotFound(string filePath, string message)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(message: message, fileName: filePath);
        }

        public static void FileNotFound(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"{filePath} path does not exist.", fileName: filePath);
        }

        public static void ArgumentLengthIsNotZero(int arrayLength, string argumentName, string message)
        {
            if (arrayLength == 0)
                throw new ArgumentException(message: message, paramName: argumentName);
        }

        public static void ArgumentHasNotValue<T>(T? argumentValue, string paramName)
            where T : struct
        {
            if (!argumentValue.HasValue)
                throw new ArgumentNullException(paramName: paramName, message: $"{paramName} has not value.");
        }

        public static void ArgumentHasNotValue<T>(T? argumentValue, string paramName, string message)
            where T : struct
        {
            if (!argumentValue.HasValue)
                throw new ArgumentNullException(paramName: paramName, message: message);
        }

        public static void ArgumentsNotEqual(string argumentValue1, string argumentValue2, string paramName, string message)
        {
            if (argumentValue1 != argumentValue2)
                throw new ArgumentNullException(paramName: paramName, message: message);
        }

        public static void ArgumentGreaterThanZero(int argumentValue, string message)
        {
            if (argumentValue <= 0)
                throw new ArgumentOutOfRangeException(paramName: null, message: message);
        }

    }
}
