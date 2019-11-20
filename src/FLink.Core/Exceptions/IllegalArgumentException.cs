using System;

namespace FLink.Core.Exceptions
{
    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException()
        {
        }

        public IllegalArgumentException(string message)
            : base(message)
        {
        }

        public IllegalArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
