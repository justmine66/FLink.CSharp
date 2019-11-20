using FLink.Core.Api.Common.Functions;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A <see cref="IStreamOperator{TOutput}"/> for executing <see cref="IFlatMapFunction{TInput,TOutput}"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class StreamFlatMap<TInput, TOutput> : AbstractUdfStreamOperator<TOutput, IFlatMapFunction<TInput, TOutput>>, IOneInputStreamOperator<TInput, TOutput>
    {
        private TimestampedCollector<TOutput> _collector;

        public StreamFlatMap(IFlatMapFunction<TInput, TOutput> userFunction)
            : base(userFunction)
            => ChainingStrategy = ChainingStrategy.Always;

        public override void Open()
        {
            base.Open();
            _collector = new TimestampedCollector<TOutput>(Output);
        }

        public void ProcessElement(StreamRecord<TInput> element)
        {
            _collector.SetTimestamp(element);
            UserFunction.FlatMap(element.Value, _collector);
        }
    }
}
