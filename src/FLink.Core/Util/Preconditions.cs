using System;
using FLink.Core.Exceptions;

namespace FLink.Core.Util
{
    /// <summary>
    /// A collection of static utility methods to validate input.
    /// </summary>
    public sealed class Preconditions
    {
        public static T CheckNotNull<T>(T reference)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            return reference;
        }

        public static T CheckNotNull<T>(T reference, string errorMessage)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference), errorMessage);

            return reference;
        }

        public static void CheckState(bool condition)
        {
            if (!condition)
                throw new IllegalStateException();
        }

        public static void CheckState(bool condition, string errorMessage)
        {
            if (!condition)
                throw new IllegalStateException(errorMessage);
        }

        public static void CheckArgument(bool condition)
        {
            if (!condition)
            {
                throw new IllegalArgumentException();
            }
        }

        public static void CheckArgument(bool condition, string errorMessage)
        {
            if (!condition)
            {
                throw new IllegalArgumentException(errorMessage);
            }
        }
    }
}
