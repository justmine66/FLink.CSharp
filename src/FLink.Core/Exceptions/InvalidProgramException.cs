using System;

namespace FLink.Core.Exceptions
{
    /// <summary>
    /// An exception thrown to indicate that the composed program is invalid. Examples of invalid programs are operations where crucial parameters are omitted, or functions where the input type and the type signature do not match.
    /// </summary>
    public class InvalidProgramException : RuntimeException
    {
        public InvalidProgramException()
        {
        }

        public InvalidProgramException(string message)
            : base(message)
        {
        }

        public InvalidProgramException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
