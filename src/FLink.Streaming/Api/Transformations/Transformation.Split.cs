using FLink.Core.Api.Dag;
using FLink.Streaming.Api.DataStreams;
using System.Collections.Generic;
using FLink.Streaming.Api.Collectors.Selectors;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This transformation represents a split of one <see cref="DataStream{TElement}"/> into several <see cref="DataStream{TElement}"/>s using an <see cref=""/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SplitTransformation<T> : Transformation<T>
    {
        public Transformation<T> Input { get; }
        public IOutputSelector<T> OutputSelector { get; }

        public SplitTransformation(Transformation<T> input, IOutputSelector<T> outputSelector)
            : base("Split", input.OutputType, input.Parallelism)
        {
            Input = input;
            OutputSelector = outputSelector;
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
