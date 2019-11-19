using System;
using System.Net;

namespace FLink.Runtime.IO.Network.Netty.Exceptions
{
    public class RemoteTransportException : TransportException
    {
        public RemoteTransportException(string message, SocketAddress address) : base(message, address)
        {
        }

        public RemoteTransportException(string message, SocketAddress address, Exception innerException) : base(message, address, innerException)
        {
        }
    }
}
