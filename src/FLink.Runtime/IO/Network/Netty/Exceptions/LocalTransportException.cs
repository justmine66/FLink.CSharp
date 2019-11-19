using System;
using System.Net;

namespace FLink.Runtime.IO.Network.Netty.Exceptions
{
    public class LocalTransportException : TransportException
    {
        public LocalTransportException(string message, SocketAddress address) : base(message, address)
        {
        }

        public LocalTransportException(string message, SocketAddress address, Exception innerException) : base(message, address, innerException)
        {
        }
    }
}
