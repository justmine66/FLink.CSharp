using FLink.Core.Api.Common.Functions;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A <see cref="IStreamOperator{TOutput}"/> for executing <see cref="IMapFunction{TInput,TOutput}"/>.
    /// </summary>
    public class StreamMap<TInput, TOutput> : AbstractUdfStreamOperator<TOutput, IMapFunction<TInput, TOutput>>, IOneInputStreamOperator<TInput, TOutput>
    {
        public StreamMap(IMapFunction<TInput, TOutput> userFunction)
            : base(userFunction)
        {
            ChainingStrategy = ChainingStrategy.Always;
        }

        public void ProcessElement(StreamRecord<TInput> element)
        {
            var mapped = UserFunction.Map(element.Value);

            //Output.Collect(element.Replace(mapped));
        }
    }
}
