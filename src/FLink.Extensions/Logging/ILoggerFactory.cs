using System;

namespace FLink.Extensions.Logging
{
    /// <summary>
    /// Represents a type used to configure the logging system and create instances of <see cref="ILogger" />.
    /// </summary>
    public interface ILoggerFactory : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.</returns>
        ILogger CreateLogger(string categoryName);

        /// <summary>Create a logger with the given type.
        /// </summary>
        ILogger CreateLogger(Type type);
    }
}
