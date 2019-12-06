using System.Collections.Generic;
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
        public Transformation<T> Input { get; }
        public IStreamOperatorFactory<object> OperatorFactory { get; }

        public SinkTransformation(
            Transformation<T> input,
            string name,
            StreamSink<T> @operator,
            int parallelism)
            : this(input, name, SimpleOperatorFactory<object>.Of(@operator), parallelism)
        {
        }

        public SinkTransformation(
            Transformation<T> input,
            string name,
            IStreamOperatorFactory<object> operatorFactory,
            int parallelism)
            : base(name, null, parallelism)
        {
            Input = input;
            OperatorFactory = operatorFactory;
        }

        public IStreamOperator<T> Operator => (OperatorFactory as SimpleOperatorFactory<T>)?.Operator;

        public override void SetChainingStrategy(ChainingStrategy strategy) => OperatorFactory.ChainingStrategy = strategy;

        public override IList<Transformation<object>> TransitivePredecessors
        {
            get
            {
                var result = new List<Transformation<dynamic>> { this as Transformation<dynamic> };
                result.AddRange(Input.TransitivePredecessors);
                return result;
            }
        }
    }
}
