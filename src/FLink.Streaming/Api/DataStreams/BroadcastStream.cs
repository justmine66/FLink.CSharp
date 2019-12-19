using System.Collections.Generic;
using System.Linq;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Environments;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// A <see cref="BroadcastStream{TElement}"/> is a stream with <see cref="IBroadcastState{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of input/output elements.</typeparam>
    public class BroadcastStream<TElement>
    {
        private readonly DataStream<TElement> _inputStream;

        protected BroadcastStream(DataStream<TElement> input, StreamExecutionEnvironment environment, params MapStateDescriptor<object, object>[] broadcastStateDescriptors)
        {
            _inputStream = input;
            Environment = environment;
            BroadcastStateDescriptors = broadcastStateDescriptors.ToList();
        }

        public TypeInformation<TElement> Type => _inputStream.Type;

        public TFunction Clean<TFunction>(TFunction f) => Environment.Clean(f);

        public Transformation<TElement> Transformation => _inputStream.Transformation;
        public List<MapStateDescriptor<object, object>> BroadcastStateDescriptors { get; }
        public StreamExecutionEnvironment Environment { get; }
    }
}
