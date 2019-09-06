using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// A <see cref="Transformation{T}"/> that creates a physical operation. It enables setting <see cref="ChainingStrategy"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements that this source produces</typeparam>
    public abstract class PhysicalTransformation<T> : Transformation<T>
    {
        protected PhysicalTransformation(
            string name,
            TypeInformation<T> outputType,
            int parallelism) 
            : base(name, outputType, parallelism)
        {
        }

        public abstract void SetChainingStrategy(ChainingStrategy strategy);
    }
}
