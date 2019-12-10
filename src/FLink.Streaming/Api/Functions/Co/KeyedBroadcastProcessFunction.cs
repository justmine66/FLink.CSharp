using System;
using FLink.Core.Api.Common.State;
using FLink.Core.Util;
using FLink.Runtime.State;
using FLink.Streaming.Api.DataStreams;

namespace FLink.Streaming.Api.Functions.Co
{
    /// <summary>
    /// A function to be applied to a <see cref="BroadcastConnectedStream{TInput1,TInput2}"/> that connects <see cref="BroadcastStream{T}"/>, i.e. a stream with broadcast state, with a <see cref="KeyedStream{TElement,TKey}"/>.
    /// The stream with the broadcast state can be created using the <see cref="KeyedStream{TElement,TKey}.Broadcast"/>
    /// </summary>
    /// <typeparam name="TKey">The key type of the input keyed stream.</typeparam>
    /// <typeparam name="TInput1">The input type of the non-broadcast side.</typeparam>
    /// <typeparam name="TInput2">The input type of the broadcast side.</typeparam>
    /// <typeparam name="TOutput">The output type of the operator.</typeparam>
    public abstract class KeyedBroadcastProcessFunction<TKey, TInput1, TInput2, TOutput> : BaseBroadcastProcessFunction
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

        /// <summary>
        /// Called when a timer set using <see cref="ITimerService"/> fires.
        /// </summary>
        /// <param name="timestamp">The timestamp of the firing timer.</param>
        /// <param name="context">A <see cref="Context"/> that allows querying the timestamp of the element, querying the <see cref="TimeDomain"/> of the firing timer and getting a <see cref="ITimerService"/> for registering timers and querying the time. The context is only valid during the invocation of this method, do not store it.</param>
        /// <param name="output">The collector to emit resulting elements to</param>
        /// <exception cref="Exception">The function may throw exceptions which cause the streaming program  to fail and go into recovery.</exception>
        public virtual void OnTimer(long timestamp, OnTimerContext context, ICollector<TOutput> output) { }

        /// <summary>
        /// A base <see cref="BaseContext"/> available to the broadcasted stream side of a <see cref="BroadcastConnectedStream{TInput1,TInput2}"/>.
        /// Apart from the basic functionality of a <see cref="BaseContext"/>, this also allows to get and update the elements stored in the <see cref="IBroadcastState{TKey,TValue}"/>.
        /// In other words, it gives read/write access to the broadcast state.
        /// </summary>
        public new abstract class Context : BaseBroadcastProcessFunction.Context
        {
            /// <summary>
            /// Applies the provided <paramref name="function"/> to the state associated with the provided <see cref="StateDescriptor{TState,TValue}"/>.
            /// </summary>
            /// <typeparam name="TStateValue"></typeparam>
            /// <typeparam name="TState"></typeparam>
            /// <param name="stateDescriptor">the descriptor of the state to be processed.</param>
            /// <param name="function">the function to be applied.</param>
            public abstract void ApplyToKeyedState<TStateValue, TState>(StateDescriptor<TState, TStateValue> stateDescriptor, IKeyedStateFunction<TKey, TState> function)
                where TState : IState;
        }

        /// <summary>
        /// A base <see cref="BaseContext"/> available to the broadcasted stream side of a <see cref="BroadcastConnectedStream{TInput1,TInput2}"/>.
        /// Apart from the basic functionality of a <see cref="BaseContext"/>, this also allows to get the elements(a read-only <see cref="IEnumerable{T}"/>) stored in the <see cref="IBroadcastState{TKey,TValue}"/>.
        /// </summary>
        public new abstract class ReadOnlyContext : BaseBroadcastProcessFunction.ReadOnlyContext
        {
            /// <summary>
            /// A <see cref="ITimerService"/> for querying time and registering timers.
            /// </summary>
            public abstract ITimerService TimerService { get; }

            /// <summary>
            /// Get key of the element being processed.
            /// </summary>
            public abstract TKey CurrentKey { get; }
        }

        /// <summary>
        /// Information available in an invocation of <see cref="KeyedBroadcastProcessFunction{TKey,TInput1,TInput2,TOutput}.OnTimer"/>.
        /// </summary>
        public abstract class OnTimerContext : Context
        {
            /// <summary>
            /// The <see cref="TimeDomain"/> of the firing timer.
            /// </summary>
            public abstract TimeDomain TimeDomain { get; }
        }
    }
}
