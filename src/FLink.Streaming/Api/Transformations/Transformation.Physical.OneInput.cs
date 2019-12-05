using System.Collections.Generic;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This Transformation represents the application of a <see cref="IOneInputStreamOperator{TInput,TOutput}"/> to one input <see cref="Transformation{T}"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the elements in the input <see cref="Transformation{T}"/></typeparam>
    /// <typeparam name="TOutput">The type of the elements that result from this <see cref="OneInputTransformation{TInput,TOutput}"/></typeparam>
    public class OneInputTransformation<TInput, TOutput> : PhysicalTransformation<TOutput>
    {
        /// <summary>
        /// Gets the input <see cref="Transformation{T}"/> of this <see cref="OneInputTransformation{TInput,TOutput}"/>.
        /// </summary>
        public Transformation<TInput> Input { get; }

        /// <summary>
        /// Gets the <see cref="TypeInformation{TType}"/> for the elements of the input.
        /// </summary>
        public TypeInformation<TInput> InputType => Input.OutputType;

        public IOneInputStreamOperator<TInput, TOutput> Operator =>
            OperatorFactory is SimpleOperatorFactory<TOutput> factory
                ? factory.Operator as IOneInputStreamOperator<TInput, TOutput>
                : null;

        /// <summary>
        /// Returns the <see cref="IStreamOperatorFactory{T}"/> of this Transformation.
        /// </summary>
        public IStreamOperatorFactory<TOutput> OperatorFactory { get; }

        /// <summary>
        /// Gets and sets the <see cref="IKeySelector{TObject,TKey}"/> that must be used for partitioning keyed state of this operation.
        /// </summary>
        public IKeySelector<TInput, object> StateKeySelector { get; set; }

        public TypeInformation<object> StateKeyType { get; set; }

        /// <summary>
        /// Creates a new <see cref="OneInputTransformation{TInput,TOutput}"/> from the given input and operator.
        /// </summary>
        /// <param name="input">The input <see cref="Transformation{T}"/></param>
        /// <param name="name">The name of the <see cref="Transformation{T}"/>, this will be shown in Visualizations and the Log</param>
        /// <param name="operator"></param>
        /// <param name="outputType">The type of the elements produced by this <see cref="OneInputTransformation{TInput,TOutput}"/></param>
        /// <param name="parallelism">The parallelism of this <see cref="OneInputTransformation{TInput,TOutput}"/></param>
        public OneInputTransformation(
            Transformation<TInput> input,
            string name,
            IOneInputStreamOperator<TInput, TOutput> @operator,
            TypeInformation<TOutput> outputType,
            int parallelism)
            : this(input, name, SimpleOperatorFactory<TOutput>.Of(@operator), outputType, parallelism)
        {
        }

        public OneInputTransformation(
            Transformation<TInput> input,
            string name,
            IStreamOperatorFactory<TOutput> operatorFactory,
            TypeInformation<TOutput> outputType,
            int parallelism) : base(name, outputType, parallelism)
        {
            Input = input;
            OperatorFactory = operatorFactory;
        }

        public override void SetChainingStrategy(ChainingStrategy strategy) => OperatorFactory.ChainingStrategy = strategy;

        public override IList<Transformation<dynamic>> TransitivePredecessors
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
