using System;

namespace FLink.Core.Exceptions
{
    public class FLinkRuntimeException : RuntimeException
    {
        public FLinkRuntimeException()
        {
        }

        public FLinkRuntimeException(string message)
            : base(message)
        {
        }

        public FLinkRuntimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
