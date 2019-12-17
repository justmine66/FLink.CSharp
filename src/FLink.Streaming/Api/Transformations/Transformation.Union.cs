using FLink.Core.Api.Dag;
using System.Collections.Generic;
using FLink.Core.Exceptions;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This transformation represents a union of several input <see cref="Transformation{TElement}"/>.
    /// This does not create a physical operation, it only affects how upstream operations are connected to downstream operations.
    /// </summary>
    /// <typeparam name="T">The type of the elements that result from this <see cref="Transformation{TElement}"/>.</typeparam>
    public class UnionTransformation<T> : Transformation<T>
    {
        /// <summary>
        /// Returns the list of input <see cref="Transformation{TElement}"/>s.
        /// </summary>
        public IList<Transformation<T>> Inputs { get; }

        public UnionTransformation(IList<Transformation<T>> inputs)
            : base("Union", inputs[0].OutputType, inputs[0].Parallelism)
        {
            foreach (var input in inputs)
            {
                if (!input.OutputType.Equals(OutputType))
                {
                    throw new UnSupportedOperationException("Type mismatch in input " + input);
                }
            }

            Inputs = inputs;
        }

        public override IList<Transformation<dynamic>> TransitivePredecessors
        {
            get
            {
                var result = new List<Transformation<dynamic>> { this as Transformation<dynamic> };
                foreach (var transformation in Inputs)
                {
                    result.AddRange(transformation.TransitivePredecessors);
                }

                return result;

            }
        }
    }
}
