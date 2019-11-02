using System;
using System.IO;

namespace FLink.Runtime.State
{
    public class BackendBuildingException : IOException
    {
        public BackendBuildingException() { }

        public BackendBuildingException(string message)
            : base(message) { }

        public BackendBuildingException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
