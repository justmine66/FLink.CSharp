using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;

namespace FLink.Streaming.Api.Functions
{
    /// <summary>
    /// A keyed function that processes elements of a stream.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TInput">Type of the input elements.</typeparam>
    /// <typeparam name="TOutput">Type of the output elements.</typeparam>
    public abstract class KeyedProcessFunction<TKey, TInput, TOutput> : AbstractRichFunction
    {
        /// <summary>
        /// Process one element from the input stream.
        /// his function can output zero or more elements using the <see cref="ICollector{TRecord}"/> parameter and also update internal state or set timers using the <see cref="Context"/> parameter.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="context"></param>
        /// <param name="output">The collector for returning result values.</param>
        public abstract void ProcessElement(TInput value, Context context, ICollector<TOutput> output);

        /// <summary>
        /// Called when a timer set using <see cref="ITimerService"/> fires.
        /// </summary>
        /// <param name="timestamp">The timestamp of the firing timer.</param>
        /// <param name="ctx">Allows querying the timestamp, the <see cref="TimeDomain"/>, and the key of the firing timer and getting a <see cref="TimeDomain"/> for registering timers and querying the time. The context is only valid during the invocation of this method, do not store it.</param>
        /// <param name="output">The collector for returning result values.</param>
        public abstract void OnTimer(long timestamp, OnTimerContext ctx, ICollector<TOutput> output);

        public abstract class Context
        {
            /// <summary>
            /// Timestamp of the element currently being processed or timestamp of a firing timer.
            /// This might be {@code null}, for example if the time characteristic of your program is set to <see cref="TimeCharacteristic.ProcessingTime"/>.
            /// </summary>
            /// <returns></returns>
            public abstract long Timestamp { get; }

            /// <summary>
            /// A <see cref="ITimerService"/> for querying time and registering timers.
            /// </summary>
            public abstract ITimerService TimerService { get; }

            /// <summary>
            /// Emits a record to the side output identified.
            /// </summary>
            /// <typeparam name="T">The record type.</typeparam>
            /// <param name="outputTag">Identifies the side output to emit to.</param>
            /// <param name="value">The record to emit.</param>
            public abstract void Output<T>(OutputTag<T> outputTag, T value);

            /// <summary>
            /// Get key of the element being processed.
            /// </summary>
            /// <returns></returns>
            public abstract TKey CurrentKey { get; }
        }

        public abstract class OnTimerContext : Context
        {
            /// <summary>
            /// The <see cref="TimeDomain"/> of the firing timer.
            /// </summary>
            public abstract TimeDomain TimeDomain { get; }
        }
    }
}
