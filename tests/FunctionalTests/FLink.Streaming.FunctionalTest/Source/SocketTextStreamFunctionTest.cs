using FLink.Streaming.Api.Functions.Source;
using Xunit;

namespace FLink.Streaming.FunctionalTest.Source
{
    public class SocketTextStreamFunctionTest
    {
        [Fact]
        public void ReadAllMessages()
        {
            var function = new SocketTextStreamFunction("localhost", 5000, "\n", 3);
            function.Run(new DummySourceContext());
        }
    }
}
