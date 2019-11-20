using System;

namespace FLink.Core.Exceptions
{
    public class RuntimeException : Exception
    {
        public RuntimeException()
        {
        }

        public RuntimeException(string message)
            : base(message)
        {
        }

        public RuntimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
