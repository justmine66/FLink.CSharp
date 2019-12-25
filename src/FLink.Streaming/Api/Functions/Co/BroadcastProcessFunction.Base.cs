using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.States;
using FLink.Core.Util;
using FLink.Streaming.Api.DataStreams;

namespace FLink.Streaming.Api.Functions.Co
{
    /// <summary>
    /// The base class containing the functionality available to all broadcast process function.
    /// These include the <see cref="BroadcastProcessFunction{TInput1,TInput2,TOutput}"/> and the <see cref="KeyedBroadcastProcessFunction{TKey,TInput1,TInput2,TOutput}"/>n}.
    /// </summary>
    public abstract class BaseBroadcastProcessFunction : AbstractRichFunction
    {
        /// <summary>
        /// The base context available to all methods in a broadcast process function.
        /// These include the <see cref="BroadcastProcessFunction{TInput1,TInput2,TOutput}"/> and the <see cref="KeyedBroadcastProcessFunction{TKey,TInput1,TInput2,TOutput}"/>n}.
        /// </summary>
        public abstract class BaseContext
        {
            /// <summary>
            /// Timestamp of the element currently being processed or timestamp of a firing timer.
            /// This might be null, for example if the time characteristic of your program is set to <see cref="TimeCharacteristic.ProcessingTime"/>.
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
            /// Gets the current processing time.
            /// </summary>
            public abstract long CurrentProcessingTime { get; }

            /// <summary>
            /// Gets the current event-time watermark.
            /// </summary>
            public abstract long CurrentWatermark { get; }
        }

        /// <summary>
        /// A base <see cref="BaseContext"/> available to the broadcasted stream side of a <see cref="BroadcastConnectedStream{TInput1,TInput2}"/>.
        /// Apart from the basic functionality of a <see cref="BaseContext"/>, this also allows to get and update the elements stored in the <see cref="IBroadcastState{TKey,TValue}"/>.
        /// In other words, it gives read/write access to the broadcast state.
        /// </summary>
        public abstract class Context : BaseContext
        { 
            /// <summary>
            /// Fetches the <see cref="IBroadcastState{TKey,TValue}"/> with the specified name.
            /// </summary>
            /// <typeparam name="TKey"></typeparam>
            /// <typeparam name="TValue"></typeparam>
            /// <param name="stateDescriptor">the <see cref="MapStateDescriptor{TK,TV}"/> of the state to be fetched.</param>
            /// <Gets></Gets>
            public abstract IBroadcastState<TKey, TValue> GetBroadcastState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateDescriptor);
        }

        /// <summary>
        /// A base <see cref="BaseContext"/> available to the broadcasted stream side of a <see cref="BroadcastConnectedStream{TInput1,TInput2}"/>.
        /// Apart from the basic functionality of a <see cref="BaseContext"/>, this also allows to get the elements(a read-only <see cref="IEnumerable{T}"/>) stored in the <see cref="IBroadcastState{TKey,TValue}"/>.
        /// </summary>
        public abstract class ReadOnlyContext : BaseContext
        {
            /// <summary>
            /// Fetches a read-only view of the broadcast state with the specified name.
            /// </summary>
            /// <typeparam name="TKey"></typeparam>
            /// <typeparam name="TValue"></typeparam>
            /// <param name="stateDescriptor">the <see cref="MapStateDescriptor{TK,TV}"/> of the state to be fetched.</param>
            /// <Gets></Gets>
            public abstract IReadOnlyBroadcastState<TKey, TValue> GetBroadcastState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateDescriptor);
        }
    }
}
