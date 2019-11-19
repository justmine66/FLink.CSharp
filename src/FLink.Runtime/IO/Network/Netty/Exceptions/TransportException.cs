using System;
using System.IO;
using System.Net;

namespace FLink.Runtime.IO.Network.Netty.Exceptions
{
    public abstract class TransportException : IOException
    {
        protected TransportException(string message, SocketAddress address)
            : this(message, address, null) { }

        protected TransportException(string message, SocketAddress address, Exception innerException)
            : base(message, innerException) => Address = address;

        public SocketAddress Address { get; }
    }
}
