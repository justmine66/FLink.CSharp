using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Streaming.Api.Environment;
using System.Collections.Generic;
using System.Linq;

namespace FLink.Streaming.Api.DataStream
{
    public class BroadcastStream<T>
    {
        private readonly DataStream<T> _inputStream;
        private readonly StreamExecutionEnvironment _environment;
        private readonly List<MapStateDescriptor<object, object>> _broadcastStateDescriptors;

        protected BroadcastStream(DataStream<T> input, StreamExecutionEnvironment environment, params MapStateDescriptor<object, object>[] broadcastStateDescriptors)
        {
            _inputStream = input;
            _environment = environment;
            _broadcastStateDescriptors = broadcastStateDescriptors.ToList();
        }
    }
}
