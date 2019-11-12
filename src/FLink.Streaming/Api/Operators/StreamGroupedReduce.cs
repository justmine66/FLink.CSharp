using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Streaming.Api.DataStreams;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecord;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A <see cref="IStreamOperator{TOutput}"/> for executing a <see cref="IReduceFunction{TElement}"/> on a <see cref="KeyedStream{TElement,TKey}"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public class StreamGroupedReduce<TInput> : AbstractUdfStreamOperator<TInput, IReduceFunction<TInput>>, IOneInputStreamOperator<TInput, TInput>
    {
        private static readonly string StateName = "_op_state";

        private readonly TypeSerializer<TInput> _serializer;
        private IValueState<TInput> _values;

        public StreamGroupedReduce(IReduceFunction<TInput> userFunction, TypeSerializer<TInput> serializer)
            : base(userFunction) => _serializer = serializer;

        public override void Open()
        {
            base.Open();
            var stateId = new ValueStateDescriptor<TInput>(StateName, _serializer);
            _values = GetPartitionedState(stateId);
        }

        public void ProcessElement(StreamRecord<TInput> element)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessWatermark(Watermark mark)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessLatencyMarker(LatencyMarker latencyMarker)
        {
            throw new System.NotImplementedException();
        }
    }
}
