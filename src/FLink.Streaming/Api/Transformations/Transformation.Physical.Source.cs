using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Core.Util;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Operators;
using System.Collections.Generic;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This represents a Source. This does not actually transform anything since it has no inputs but
    /// it is the root <see cref="Transformation{T}"/> of any topology.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements that this source produces</typeparam>
    public class SourceTransformation<TElement> : PhysicalTransformation<TElement>
    {
        /// <summary>
        /// Returns the <see cref="IStreamOperatorFactory{TOutput}"/> of this <see cref="SourceTransformation{TElement}"/>.
        /// </summary>
        public IStreamOperatorFactory<TElement> OperatorFactory { get; }

        /// <summary>
        /// Creates a new <see cref="SourceTransformation{T}"/> from the given operator.
        /// </summary>
        /// <param name="name">The name of the <see cref="SourceTransformation{T}"/>, this will be shown in Visualizations and the Log</param>
        /// <param name="operator">The <see cref="StreamSource{TOut,TSrc}"/> that is the operator of this Transformation</param>
        /// <param name="outputType">The type of the elements produced by this <see cref="SourceTransformation{T}"/></param>
        /// <param name="parallelism">The parallelism of this <see cref="SourceTransformation{T}"/></param>
        public SourceTransformation(
            string name,
            StreamSource<TElement, ISourceFunction<TElement>> @operator,
            TypeInformation<TElement> outputType,
            int parallelism) : this(name, new SimpleOperatorFactory<TElement>(@operator), outputType, parallelism)
        {
        }

        public SourceTransformation(
            string name,
            IStreamOperatorFactory<TElement> operatorFactory,
            TypeInformation<TElement> outputType,
            int parallelism) : base(name, outputType, parallelism)
        {
            OperatorFactory = operatorFactory;
        }

        public IStreamOperator<TElement> Operator => (OperatorFactory as SimpleOperatorFactory<TElement>)?.Operator;

        public override void SetChainingStrategy(ChainingStrategy strategy) => OperatorFactory.ChainingStrategy = strategy;

        public override IList<Transformation<dynamic>> TransitivePredecessors => SingletonList<Transformation<dynamic>>.Instance;
    }
}
