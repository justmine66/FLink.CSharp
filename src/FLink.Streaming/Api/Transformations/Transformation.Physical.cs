using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// A <see cref="Transformation{T}"/> that creates a physical operation. It enables setting <see cref="ChainingStrategy"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements that this source produces</typeparam>
    public abstract class PhysicalTransformation<TElement> : Transformation<TElement>
    {
        /// <summary>
        ///  Creates a new <see cref="Transformation{TElement}"/> with the given name, output type and parallelism.
        /// </summary>
        /// <param name="name">The name of the <see cref="Transformation{TElement}"/>, this will be shown in Visualizations and the Log</param>
        /// <param name="outputType">The output type of this <see cref="Transformation{TElement}"/></param>
        /// <param name="parallelism">The parallelism of this <see cref="Transformation{TElement}"/></param>
        protected PhysicalTransformation(
            string name,
            TypeInformation<TElement> outputType,
            int parallelism)
            : base(name, outputType, parallelism)
        {
        }

        /// <summary>
        /// Sets the chaining strategy of this <see cref="Transformation{TElement}"/>.
        /// </summary>
        /// <param name="strategy"></param>
        public abstract void SetChainingStrategy(ChainingStrategy strategy);
    }
}
