using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace FLink.Extensions.Transport.DotNetty
{
    public class SocketClient
    {
        private readonly string _host;
        private readonly int _port;

        public SocketClient(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public async Task RunAsync(Action<string> onReceived, Action<Exception> onException, Action onReadComplete)
        {
            var group = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;

                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                        pipeline.AddLast("SocketTextStream", new SocketTextStreamHandler(onReceived, onReadComplete));
                    }));

                var clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(_host), _port));

                Console.ReadLine();

                await clientChannel.CloseAsync();
            }
            catch (Exception e)
            {
                onException(e);
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }
    }
}
