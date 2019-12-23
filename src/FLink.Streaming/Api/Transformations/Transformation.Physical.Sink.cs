using System.Collections.Generic;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This Transformation represents a Sink.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the input <see cref="SinkTransformation{T}"/></typeparam>
    public class SinkTransformation<TElement> : PhysicalTransformation<object>
    {
        public Transformation<TElement> Input { get; }
        public IStreamOperatorFactory<object> OperatorFactory { get; }

        /// <summary>
        /// Sets the <see cref="IKeySelector{TObject,TKey}"/> that must be used for partitioning keyed state of this Sink.
        /// </summary>
        public IKeySelector<TElement, object> StateKeySelector { get; set; }

        public TypeInformation<object> StateKeyType { get; set; }

        public SinkTransformation(
            Transformation<TElement> input,
            string name,
            StreamSink<TElement> @operator,
            int parallelism)
            : this(input, name, SimpleOperatorFactory<object>.Of(@operator), parallelism)
        {
        }

        public SinkTransformation(
            Transformation<TElement> input,
            string name,
            IStreamOperatorFactory<object> operatorFactory,
            int parallelism)
            : base(name, null, parallelism)
        {
            Input = input;
            OperatorFactory = operatorFactory;
        }

        public IStreamOperator<TElement> Operator => (OperatorFactory as SimpleOperatorFactory<TElement>)?.Operator;

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
