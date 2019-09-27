using System;

namespace FLink.Extensions.Logging
{
    public class EmptyLoggerFactory : ILoggerFactory
    {
        private static readonly EmptyLogger Logger = new EmptyLogger();

        public void Dispose() { }

        public ILogger CreateLogger(string categoryName) => Logger;

        public ILogger CreateLogger(Type type) => Logger;
    }
}
