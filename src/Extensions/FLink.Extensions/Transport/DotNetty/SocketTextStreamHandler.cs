using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using FLink.Extensions.DependencyInjection;
using FLink.Extensions.Logging;

namespace FLink.Extensions.Transport.DotNetty
{
    public class SocketTextStreamHandler : ChannelHandlerAdapter
    {
        readonly IByteBuffer _initialMessage;
        private readonly ILogger<SocketTextStreamHandler> _logger;
        private readonly Action<string> _onRead;
        private readonly Action _onReadComplete;

        public SocketTextStreamHandler(Action<string> onRead, Action onReadComplete = null)
        {
            _initialMessage = Unpooled.Buffer(256);
            _logger = ObjectContainer.Current.GetService<ILogger<SocketTextStreamHandler>>();
            _onRead = onRead;
            _onReadComplete = onReadComplete;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is IByteBuffer byteBuffer)
                _onRead(byteBuffer.ToString(Encoding.UTF8));
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
            _onReadComplete?.Invoke();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
