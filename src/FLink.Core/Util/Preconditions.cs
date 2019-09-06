using System;

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
    }
}
