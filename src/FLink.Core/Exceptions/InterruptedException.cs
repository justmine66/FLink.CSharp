using System;

namespace FLink.Core.Exceptions
{
    public class InterruptedException : Exception
    {
        public InterruptedException()
        {
        }

        public InterruptedException(string message)
            : base(message)
        {
        }

        public InterruptedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
