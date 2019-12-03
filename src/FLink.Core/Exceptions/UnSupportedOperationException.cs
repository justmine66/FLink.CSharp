using System;

namespace FLink.Core.Exceptions
{
    public class UnSupportedOperationException : Exception
    {
        public UnSupportedOperationException()
        {
        }

        public UnSupportedOperationException(string message)
            : base(message)
        {
        }

        public UnSupportedOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
