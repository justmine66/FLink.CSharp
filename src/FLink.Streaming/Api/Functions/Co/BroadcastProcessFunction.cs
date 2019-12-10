using System;
using FLink.Core.Util;
using FLink.Streaming.Api.DataStreams;

namespace FLink.Streaming.Api.Functions.Co
{
    /// <summary>
    /// A function to be applied to a <see cref="BroadcastConnectedStream{TInput1,TInput2}"/>.
    /// </summary>
    /// <typeparam name="TInput1">The input type of the non-broadcast side.</typeparam>
    /// <typeparam name="TInput2">The input type of the broadcast side.</typeparam>
    /// <typeparam name="TOutput">The output type of the operator.</typeparam>
    public abstract class BroadcastProcessFunction<TInput1, TInput2, TOutput> : BaseBroadcastProcessFunction
    {
        /// <summary>
        /// This method is called for each element in the (non-broadcast) <see cref="DataStream{TElement}"/>.
        /// </summary>
        /// <param name="value">The stream element.</param>
        /// <param name="context">A context that allows querying the timestamp of the element, querying the current processing/event time and updating the broadcast state.
        /// The context is only valid during the invocation of this method, do not store it.</param>
        /// <param name="output">The collector to emit resulting elements to</param>
        /// <exception cref="Exception">The function may throw exceptions which cause the streaming program to fail and go into recovery.</exception>
        public abstract void ProcessElement(TInput1 value, ReadOnlyContext context, ICollector<TOutput> output);

        /// <summary>
        ///  This method is called for each element in the <see cref="BroadcastStream{TElement}"/>.
        /// </summary>
        /// <param name="value">The stream element.</param>
        /// <param name="context">A context that allows querying the timestamp of the element, querying the current processing/event time and updating the broadcast state.
        /// The context is only valid during the invocation of this method, do not store it.</param>
        /// <param name="output">The collector to emit resulting elements to</param>
        /// <exception cref="Exception">The function may throw exceptions which cause the streaming program to fail and go into recovery.</exception>
        public abstract void ProcessBroadcastElement(TInput2 value, Context context, ICollector<TOutput> output);
    }
}
