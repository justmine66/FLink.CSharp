using System;

namespace FLink.Core.Exceptions
{
    public class FLinkException : Exception
    {
        public FLinkException()
        {
        }

        public FLinkException(string message) 
            : base(message)
        {
        }

        public FLinkException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
