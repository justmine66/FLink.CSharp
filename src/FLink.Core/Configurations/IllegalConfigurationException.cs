using System;

namespace FLink.Core.Configurations
{
    /// <summary>
    /// An <see cref="IllegalConfigurationException"/> is thrown when the values in a given <see cref="Configuration"/> are not valid.
    /// </summary>
    public class IllegalConfigurationException : Exception
    {
        public IllegalConfigurationException()
        {
        }

        public IllegalConfigurationException(string message)
            : base(message)
        {
        }

        public IllegalConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
