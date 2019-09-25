using System;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This Transformation represents a Sink.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the input <see cref="SinkTransformation{T}"/></typeparam>
    public class SinkTransformation<T> : PhysicalTransformation<object>
    {
        public SinkTransformation(
            Transformation<T> input,
            string name,
            StreamSink<T> @operator,
            int parallelism)
            : this(input, name, SimpleOperatorFactory<T>.Of(@operator), parallelism)
        {
        }

        public SinkTransformation(
            Transformation<T> input,
            string name,
            IStreamOperatorFactory<T> operatorFactory,
            int parallelism)
            : base(name, null, parallelism)
        {

        }
        public SinkTransformation(string name, TypeInformation<object> outputType, int parallelism)
            : base(name, outputType, parallelism)
        {
        }

        public override void SetChainingStrategy(ChainingStrategy strategy)
        {
            throw new NotImplementedException();
        }
    }
}
