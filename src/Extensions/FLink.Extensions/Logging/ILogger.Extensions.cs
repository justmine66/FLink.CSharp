using System;

namespace FLink.Extensions.Logging
{
    /// <summary>
    /// ILogger extension methods for common scenarios.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>Formats and writes a debug log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message.</param>
        public static void LogDebug(this ILogger logger, Exception exception, string message)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, message, exception);
        }

        /// <summary>Formats and writes a debug log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogDebug(this ILogger logger, string message, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, string.Format(message, args), null);
        }

        /// <summary>Formats and writes a debug log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogDebug(this ILogger logger, Exception exception, string format, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log(LogLevel.Debug, string.Format(format, args), exception);
        }

        /// <summary>Formats and writes a trace log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogTrace(
            this ILogger logger,
            Exception exception,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Trace, string.Format(message, args), exception);
        }

        /// <summary>Formats and writes a trace log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogTrace(
            this ILogger logger,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Trace, string.Format(message, args), null);
        }

        /// <summary>Formats and writes an informational log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogInformation(
            this ILogger logger,
            Exception exception,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Information, string.Format(message, args), exception);
        }

        /// <summary>Formats and writes an informational log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogInformation(
            this ILogger logger,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Information, string.Format(message, args), null);
        }

        /// <summary>Formats and writes a warning log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogWarning(
            this ILogger logger,
            Exception exception,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Warning, string.Format(message, args), exception);
        }

        /// <summary>Formats and writes a warning log message.</summary>
        /// <param name="logger">The <see cref="ILogger" /> to write to.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogWarning(
            this ILogger logger,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Warning, string.Format(message, args), null);
        }

        /// <summary>Formats and writes an error log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogError(
            this ILogger logger,
            Exception exception,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Error, string.Format(message, args), exception);
        }

        /// <summary>Formats and writes an error log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogError(
            this ILogger logger,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Error, string.Format(message, args), null);
        }

        /// <summary>Formats and writes a critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogCritical(
            this ILogger logger,
            Exception exception,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Critical, string.Format(message, args), exception);
        }

        /// <summary>Formats and writes a critical log message.</summary>
        /// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
        /// <param name="message">Format string of the log message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void LogCritical(
            this ILogger logger,
            string message,
            params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            logger.Log<object>(LogLevel.Critical, string.Format(message, args), null);
        }
    }
}
