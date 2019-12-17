using FLink.Core.Api.Dag;
using FLink.Core.Exceptions;
using System.Collections.Generic;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This represents a feedback point in a topology.
    /// This is different from how iterations work in batch processing. Once a feedback point is defined you can connect one or several <see cref="Transformation{TElement}"/>s as a feedback edges. Operations downstream from the feedback point will receive elements from the input of this feedback point and from the feedback edges.
    /// </summary>
    /// <typeparam name="T">The type of the input elements and the feedback elements.</typeparam>
    public class FeedbackTransformation<T> : Transformation<T>
    {
        /// <summary>
        /// Returns the input <see cref="Transformation{TElement}"/> of this <see cref="FeedbackTransformation{T}"/>.
        /// </summary>
        public Transformation<T> Input { get; }

        /// <summary>
        /// Returns the list of feedback <see cref="Transformation{TElement}"/>s.
        /// </summary>
        public IList<Transformation<T>> FeedbackEdges { get; }

        /// <summary>
        /// Returns the wait time. This is the amount of time that the feedback operator keeps listening for feedback elements. Once the time expires the operation will close and will not receive further elements.
        /// </summary>
        public long WaitTime { get; }

        public FeedbackTransformation(Transformation<T> input, long waitTime)
            : base("Feedback", input.OutputType, input.Parallelism)
        {
            Input = input;
            FeedbackEdges = new List<Transformation<T>>();
            WaitTime = waitTime;
        }

        /// <summary>
        /// Adds a feedback edge. 
        /// </summary>
        /// <param name="transform">The new feedback <see cref="Transformation{TElement}"/>.</param>
        public void AddFeedbackEdge(Transformation<T> transform)
        {

            if (transform.Parallelism != Parallelism)
            {
                throw new UnSupportedOperationException(
                    $"Parallelism of the feedback stream must match the parallelism of the original stream. Parallelism of original stream: {Parallelism}; parallelism of feedback stream: {transform.Parallelism}. Parallelism can be modified using DataStream#setParallelism() method");
            }

            FeedbackEdges.Add(transform);
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
