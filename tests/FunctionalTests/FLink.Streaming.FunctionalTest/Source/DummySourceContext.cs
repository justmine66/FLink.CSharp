using System;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Watermarks;

namespace FLink.Streaming.FunctionalTest.Source
{
    public class DummySourceContext : ISourceFunctionContext<string>
    {
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Collect(string element)
        {
            Console.WriteLine(element);
        }

        public void CollectWithTimestamp(string element, long timestamp)
        {
            throw new NotImplementedException();
        }

        public void EmitWatermark(Watermark mark)
        {
            throw new NotImplementedException();
        }

        public object GetCheckpointLock()
        {
            throw new NotImplementedException();
        }

        public void MarkAsTemporarilyIdle()
        {
            throw new NotImplementedException();
        }
    }
}
