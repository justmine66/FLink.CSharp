using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.State;
using FLink.Core.Util;

namespace FLink.Streaming.Api.Functions.Co
{
    /// <summary>
    /// The base class containing the functionality available to all broadcast process function.
    /// </summary>
    public abstract class BaseBroadcastProcessFunction : AbstractRichFunction
    {
        public abstract class BaseContext
        {
            /// <summary>
            /// Timestamp of the element currently being processed or timestamp of a firing timer.
            /// This might be {@code null}, for example if the time characteristic of your program is set to <see cref="TimeCharacteristic.ProcessingTime"/>.
            /// </summary>
            public abstract long Timestamp { get; }

            /// <summary>
            /// Emits a record to the side output identified by the <see cref="OutputTag{T}"/>.
            /// </summary>
            /// <typeparam name="T">The record type to emit.</typeparam>
            /// <param name="outputTag">the <see cref="OutputTag{T}"/> that identifies the side output to emit to.</param>
            /// <param name="value">The record to emit.</param>
            public abstract void Output<T>(OutputTag<T> outputTag, T value);

            /// <summary>
            /// Returns the current processing time.
            /// </summary>
            public abstract long CurrentProcessingTime { get; }

            /// <summary>
            /// Returns the current event-time watermark.
            /// </summary>
            public abstract long CurrentWatermark { get; }
        }

        public abstract class Context : BaseContext
        {
            /// <summary>
            /// Fetches the <see cref="IBroadcastState{TKey,TValue}"/> with the specified name.
            /// </summary>
            /// <typeparam name="TKey"></typeparam>
            /// <typeparam name="TValue"></typeparam>
            /// <param name="stateDescriptor">the <see cref="MapStateDescriptor{TK,TV}"/> of the state to be fetched.</param>
            /// <returns></returns>
            public abstract IBroadcastState<TKey, TValue> GetBroadcastState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateDescriptor);
        }

        public abstract class ReadOnlyContext : BaseContext
        {
            /// <summary>
            /// Fetches a read-only view of the broadcast state with the specified name.
            /// </summary>
            /// <typeparam name="TKey"></typeparam>
            /// <typeparam name="TValue"></typeparam>
            /// <param name="stateDescriptor">the <see cref="MapStateDescriptor{TK,TV}"/> of the state to be fetched.</param>
            /// <returns></returns>
            public abstract IReadOnlyBroadcastState<TKey, TValue> GetBroadcastState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateDescriptor);
        }
    }
}
