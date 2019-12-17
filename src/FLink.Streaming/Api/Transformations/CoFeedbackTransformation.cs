using System.Collections.Generic;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Core.Exceptions;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This represents a feedback point in a topology. 
    /// </summary>
    /// <typeparam name="F"></typeparam>
    public class CoFeedbackTransformation<F> : Transformation<F>
    {
        /// <summary>
        /// Returns the list of feedback <see cref="Transformation{TElement}"/>s.
        /// </summary>
        public IList<Transformation<F>> FeedbackEdges { get; }

        /// <summary>
        /// Returns the wait time. This is the amount of time that the feedback operator keeps listening for feedback elements. Once the time expires the operation will close and will not receive further elements.
        /// </summary>
        public long WaitTime { get; }

        public CoFeedbackTransformation(int parallelism, TypeInformation<F> feedbackType, long waitTime)
            : base("CoFeedback", feedbackType, parallelism)
        {
            WaitTime = waitTime;
            FeedbackEdges = new List<Transformation<F>>();
        }

        public void AddFeedbackEdge(Transformation<F> transform)
        {
            if (transform.Parallelism != Parallelism)
            {
                throw new UnSupportedOperationException(
                    "Parallelism of the feedback stream must match the parallelism of the original" +
                    " stream. Parallelism of original stream: " + Parallelism +
                    "; parallelism of feedback stream: " + transform.Parallelism);
            }

            FeedbackEdges.Add(transform);
        }

        public override IList<Transformation<dynamic>> TransitivePredecessors => new List<Transformation<dynamic>> { this as Transformation<dynamic> };
    }
}
