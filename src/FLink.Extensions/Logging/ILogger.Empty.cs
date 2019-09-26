using System;

namespace FLink.Extensions.Logging
{
    public class EmptyLogger : ILogger
    {
        public void Log<TState>(LogLevel logLevel, TState state, Exception exception) { }

        public bool IsEnabled(LogLevel logLevel) => false;
    }
}
