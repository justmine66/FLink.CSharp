using System.Text;
using System.Threading;

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

        // Default connection timeout when connecting to the server socket (infinite).
        private const int ConnectionTimeoutTime = 0;

        private volatile bool _isRunning;

        private readonly string _hostname;
        private readonly int _port;
        private readonly string _delimiter;
        private readonly long _maxNumRetries;
        private readonly long _delayBetweenRetries;

        public SocketTextStreamFunction(string hostname, int port, string delimiter, long maxNumRetries)
            : this(hostname, port, delimiter, maxNumRetries, DefaultConnectionRetrySleep)
        {
        }

        public SocketTextStreamFunction(string hostname, int port, string delimiter, long maxNumRetries, long delayBetweenRetries)
        {
            CheckArgument(port > 0 && port < 65536, $"The {nameof(port)} is out of range");
            CheckArgument(maxNumRetries >= -1, $"The {nameof(maxNumRetries)} must be zero or larger (num retries), or -1 (infinite retries)");
            CheckArgument(delayBetweenRetries >= 0, $"The {nameof(delayBetweenRetries)} must be zero or positive");

            _hostname = CheckNotNull(hostname, "hostname must not be null");
            _port = port;
            _delimiter = delimiter;
            _maxNumRetries = maxNumRetries;
            _delayBetweenRetries = delayBetweenRetries;
        }

        public void Run(ISourceContext<string> ctx)
        {
            var buffer = new StringBuilder();
            long attempt;

            while (_isRunning)
            {

            }
        }

        public void Cancel()
        {
            _isRunning = false;
        }
    }
}
