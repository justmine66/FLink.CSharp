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

        private TypeSerializer<TInput> _serializer;
        private readonly IValueState<TInput> _values;

        public StreamGroupedReduce(IReduceFunction<TInput> userFunction, TypeSerializer<TInput> serializer)
            : base(userFunction) => _serializer = serializer;

        public override void Open()
        {
            base.Open();
            var stateId = new ValueStateDescriptor<TInput>(StateName, _serializer);
            values = getPartitionedState(stateId);
        }

        public void ProcessElement(StreamRecord<TInput> element)
        {
            var value = element.v
            var currentValue = values.value();

            if (currentValue != null)
            {
                IN reduced = userFunction.reduce(currentValue, value);
                values.update(reduced);
                output.collect(element.replace(reduced));
            }
            else
            {
                values.update(value);
                output.collect(element.replace(value));
            }
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
