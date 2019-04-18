using System;

namespace MDA.Shared
{
    public static class Assert
    {
        public static void NotNull<T>(string argumentName, T argument) where T : class
        {
            if (argument == null)
                throw new ArgumentNullException($"The {argumentName} must be provided.");
        }

        public static void NotNull<T>(string argumentName, T argument, string message) where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName, message);
        }

        public static void CharacterLengthLessThan(string argumentName, string argument, int length)
        {
            if (argument.Length <= length)
                throw new ArgumentException($"The {argumentName} must be {length} characters or less.");
        }

        public static void LengthGreaterThan(string argumentName, int value, int length)
        {
            if (value <= length)
                throw new ArgumentException($"The {argumentName} length must greater than{length}.");
        }

        public static void LengthGreaterThan(string argumentName, decimal value, decimal length)
        {
            if (value <= length)
                throw new ArgumentException($"The {argumentName} length must greater than{length}.");
        }

        public static void LengthLessThan(string argumentName, int value, int length)
        {
            if (value >= length)
                throw new ArgumentException($"The {argumentName} length must less than{length}.");
        }

        public static void LengthLessThan(string argumentName, int value, int length, string message)
        {
            if (value >= length)
                throw new ArgumentException(message);
        }

        public static void NotNullOrEmpty(string argumentName, string argument)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(argument, argumentName);
        }

        public static void Positive(int number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be positive.");
        }

        public static void Positive(long number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be positive.");
        }

        public static void Positive(long number, string argumentName, string message)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName, message);
        }

        public static void Nonnegative(long number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be non negative.");
        }

        public static void Nonnegative(int number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be non negative.");
        }

        public static void NotEmptyGuid(Guid guid, string argumentName)
        {
            if (Guid.Empty == guid)
                throw new ArgumentException(argumentName, argumentName + " should be non-empty GUID.");
        }

        public static void Equal(int expected, int actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentException($"{argumentName} expected value: {expected}, actual value: {actual}");
        }

        public static void Equal(long expected, long actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentException($"{argumentName} expected value: {expected}, actual value: {actual}");
        }

        public static void Equal(bool expected, bool actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentException($"{argumentName} expected value: {expected}, actual value: {actual}");
        }
    }
}
