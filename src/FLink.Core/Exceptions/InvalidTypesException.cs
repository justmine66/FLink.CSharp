using System;

namespace FLink.Core.Exceptions
{
    /// <summary>
    /// A special case of the <see cref="InvalidTypesException"/>, indicating that the types used in an operation are invalid or inconsistent.
    /// </summary>
    public class InvalidTypesException : InvalidProgramException
    {
        public InvalidTypesException()
        {
        }

        public InvalidTypesException(string message)
            : base(message)
        {
        }

        public InvalidTypesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
