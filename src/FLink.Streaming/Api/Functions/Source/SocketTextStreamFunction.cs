using System;
using System.Threading;
using FLink.Extensions.DependencyInjection;
using FLink.Extensions.Logging;
using FLink.Extensions.Transport.DotNetty;

namespace FLink.Streaming.Api.Functions.Source
{
    using static FLink.Core.Util.Preconditions;

    /// <summary>
    /// A source function that reads strings from a socket. The source will read bytes from the socket stream and convert them to characters, each byte individually. When the delimiter character is received, the function will output the current string, and begin a new string.
    ///
    /// The function strips trailing <i>carriage return</i> characters (\r) when the delimiter is the newline character.
    /// The function can be set to reconnect to the server socket in case that the stream is closed on the server side.
    /// </summary>
    public class SocketTextStreamFunction : ISourceFunction<string>
    {
        // Default delay between successive connection attempts.
        private const int DefaultConnectionRetrySleep = 500;

        private volatile bool _isRunning;

        private readonly string _delimiter;
        private readonly int _maxNumRetries;
        private readonly int _delayBetweenRetries;
        private readonly SocketClient _socket;
        private readonly ILogger _logger;

        public SocketTextStreamFunction(string host, int port, string delimiter, int maxNumRetries)
            : this(host, port, delimiter, maxNumRetries, DefaultConnectionRetrySleep)
        {
            _socket = new SocketClient(host, port);
            _logger = ObjectContainer.Current.GetService<ILogger<SocketTextStreamFunction>>();
        }

        public SocketTextStreamFunction(string host, int port, string delimiter, int maxNumRetries, int delayBetweenRetries)
        {
            CheckNotNull(host, "host must not be null");
            CheckArgument(port > 0 && port < 65536, $"The {nameof(port)} is out of range");
            CheckArgument(maxNumRetries >= -1, $"The {nameof(maxNumRetries)} must be zero or larger (num retries), or -1 (infinite retries)");
            CheckArgument(delayBetweenRetries >= 0, $"The {nameof(delayBetweenRetries)} must be zero or positive");

            _delimiter = delimiter;
            _maxNumRetries = maxNumRetries;
            _delayBetweenRetries = delayBetweenRetries;
        }

        public void Run(ISourceContext<string> ctx)
        {
            var attempt = 0;

            while (_isRunning)
            {
                _socket.RunAsync((message) =>
                {
                    // truncate trailing carriage return
                    if (_delimiter.Equals("\n") && message.EndsWith("\r"))
                        message = message.Substring(0, message.Length - 1);
                    ctx.Collect(message);
                }, (ex) =>
                {
                    if (!_isRunning) throw ex;

                    attempt++;
                    if (_maxNumRetries == -1 || attempt < _maxNumRetries)
                    {
                        _logger.LogWarning("Lost connection to server socket. Retrying in " + _delayBetweenRetries +
                                           " msecs...");
                        Thread.Sleep(_delayBetweenRetries * attempt);
                    }
                    else throw ex;
                }, () => _isRunning = false).Wait();
            }
        }

        public void Cancel()
        {
            _isRunning = false;
        }
    }
}
