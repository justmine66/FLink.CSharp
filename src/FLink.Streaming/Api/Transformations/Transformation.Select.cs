using System.Collections.Generic;
using FLink.Core.Api.Dag;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This transformation represents a selection of only certain upstream elements. This must follow a <see cref="SplitTransformation{T}"/> that splits elements into several logical streams with assigned names.
    /// This does not create a physical operation, it only affects how upstream operations are connected to downstream operations.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements that result from this <see cref="SelectTransformation{T}"/></typeparam>
    public class SelectTransformation<TElement> : Transformation<TElement>
    {
        /// <summary>
        /// Gets the input <see cref="Transformation{TElement}"/>.
        /// </summary>
        public Transformation<TElement> Input { get; }

        /// <summary>
        /// Gets the names of the split streams that this <see cref="SelectTransformation{T}"/> selects.
        /// </summary>
        public List<string> SelectedNames { get; }

        /// <summary>
        /// Creates a new <see cref="SelectTransformation{T}"/> from the given input that only selects the streams with the selected names.
        /// </summary>
        /// <param name="input">The input <see cref="Transformation{TElement}"/></param>
        /// <param name="selectedNames">The names from the upstream <see cref="SplitTransformation{T}"/> that this <see cref="SelectTransformation{T}"/> selects.</param>
        public SelectTransformation(Transformation<TElement> input,List<string> selectedNames)
            :base("Select", input.OutputType, input.Parallelism)
        {
            Input = input;
            SelectedNames = selectedNames;
        }

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
