using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;

namespace FLink.Streaming.Api.Functions.Co
{
    /// <summary>
    /// A function that processes elements of two keyed streams and produces a single output one.
    /// The function will be called for every element in the input streams and can produce zero or more output elements.Contrary to the <see cref="ICoFlatMapFunction{TInput1,TInput2,TOutput}"/>, this function can also query the time (both event and processing) and set timers, through the provided <see cref="Context"/>. When reacting to the firing of set timers the function can emit yet more elements.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TInput1">Type of the first input.</typeparam>
    /// <typeparam name="TInput2">Type of the second input.</typeparam>
    /// <typeparam name="TOutput">Output type.</typeparam>
    public abstract class KeyedCoProcessFunction<TKey, TInput1, TInput2, TOutput> : AbstractRichFunction
    {
        /// <summary>
        /// This method is called for each element in the first of the connected streams.
        /// This function can output zero or more elements using the <paramref name="output"/> parameter and also update internal state or set timers using the <paramref name="context"/> parameter.
        /// </summary>
        /// <param name="value">The stream element</param>
        /// <param name="context">A <see cref="Context"/> that allows querying the timestamp of the element, querying the <see cref="TimeDomain"/> of the firing timer and getting a <see cref="ITimerService"/> for registering timers and querying the time. The context is only valid during the invocation of this method, do not store it.</param>
        /// <param name="output">The collector to emit resulting elements to</param>
        /// <exception cref="Exception">The function may throw exceptions which cause the streaming program  to fail and go into recovery.</exception>
        public abstract void ProcessElement1(TInput1 value, Context context, ICollector<TOutput> output);

        /// <summary>
        /// This method is called for each element in the second of the connected streams.
        /// This function can output zero or more elements using the <paramref name="output"/> parameter and also update internal state or set timers using the <paramref name="context"/> parameter.
        /// </summary>
        /// <param name="value">The stream element</param>
        /// <param name="context">A <see cref="Context"/> that allows querying the timestamp of the element, querying the <see cref="TimeDomain"/> of the firing timer and getting a <see cref="ITimerService"/> for registering timers and querying the time. The context is only valid during the invocation of this method, do not store it.</param>
        /// <param name="output">The collector to emit resulting elements to</param>
        /// <exception cref="Exception">The function may throw exceptions which cause the streaming program  to fail and go into recovery.</exception>
        public abstract void ProcessElement2(TInput2 value, Context context, ICollector<TOutput> output);

        /// <summary>
        /// Called when a timer set using <see cref="ITimerService"/> fires.
        /// </summary>
        /// <param name="timestamp">The timestamp of the firing timer.</param>
        /// <param name="context">A <see cref="Context"/> that allows querying the timestamp of the element, querying the <see cref="TimeDomain"/> of the firing timer and getting a <see cref="ITimerService"/> for registering timers and querying the time. The context is only valid during the invocation of this method, do not store it.</param>
        /// <param name="output">The collector to emit resulting elements to</param>
        /// <exception cref="Exception">The function may throw exceptions which cause the streaming program  to fail and go into recovery.</exception>
        public virtual void OnTimer(long timestamp, OnTimerContext context, ICollector<TOutput> output) { }

        /// <summary>
        /// Information available in an invocation of <see cref="CoProcessFunction{IN1,IN2,OUT}.ProcessElement1"/> and <see cref="CoProcessFunction{IN1,IN2,OUT}.ProcessElement2"/>.
        /// </summary>
        public abstract class Context
        {
            /// <summary>
            /// Timestamp of the element currently being processed or timestamp of a firing timer.
            /// This might be null, for example if the time characteristic of your program is set to <see cref="TimeCharacteristic.ProcessingTime"/>.
            /// </summary>
            public abstract long Timestamp { get; }

            /// <summary>
            /// A <see cref="ITimerService"/> for querying time and registering timers.
            /// </summary>
            public abstract ITimerService TimerService { get; }

            /// <summary>
            /// Emits a record to the side output identified by the <see cref="OutputTag{TElement}"/>.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="outputTag">the <see cref="OutputTag{TElement}"/> that identifies the side output to emit to.</param>
            /// <param name="value">The record to emit.</param>
            public abstract void Output<T>(OutputTag<T> outputTag, T value);

            /// <summary>
            /// Get key of the element being processed.
            /// </summary>
            public abstract TKey CurrentKey { get; }
        }

        /// <summary>
        /// Information available in an invocation of  <see cref="CoProcessFunction{IN1,IN2,OUT}.OnTimer"/>
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
