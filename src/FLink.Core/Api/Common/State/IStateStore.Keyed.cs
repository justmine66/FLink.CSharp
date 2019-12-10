using System;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// This interface contains methods for registering keyed state with a managed store.
    /// </summary>
    public interface IKeyedStateStore
    {
        /// <summary>
        /// Gets a handle to the system's key/value state. The key/value state is only accessible if the function is executed on a KeyedStream. On each access, the state exposes the value for the key of the element currently processed by the function.
        /// Each function may have multiple partitioned states, addressed with different names.
        /// Because the scope of each value is the key of the currently processed element, and the elements are distributed by the Flink runtime, the system can transparently scale out and redistribute the state and KeyedStream.
        /// </summary>
        /// <typeparam name="TValue">The type of value stored in the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IValueState<TValue> GetState<TValue>(ValueStateDescriptor<TValue> stateProperties);

        /// <summary>
        /// Gets a handle to the system's key/value list state. This state is similar to the state accessed via <see cref="GetState{TValue}"/>, but is optimized for state that holds lists. One can add elements to the list, or retrieve the list as a whole.
        /// This state is only accessible if the function is executed on a KeyedStream.
        /// </summary>
        /// <typeparam name="TValue">The type of value stored in the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateProperties);

        /// <summary>
        /// Gets a handle to the system's key/value reducing state. This state is similar to the state accessed via <see cref="GetState{TValue}"/>, but is optimized for state that aggregates values.
        /// This state is only accessible if the function is executed on a KeyedStream.
        /// </summary>
        /// <typeparam name="TValue">The type of value stored in the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IReducingState<TValue> GetReducingState<TValue>(ReducingStateDescriptor<TValue> stateProperties);

        /// <summary>
        /// Gets a handle to the system's key/value aggregating state. This state is similar to the state accessed via <see cref="GetState{TValue}"/>, but is optimized for state that aggregates values with different types.
        /// This state is only accessible if the function is executed on a KeyedStream.
        /// </summary>
        /// <typeparam name="TInput">The type of the values that are added to the state.</typeparam>
        /// <typeparam name="TAccumulator">The type of the accumulator (intermediate aggregation state).</typeparam>
        /// <typeparam name="TOutput">The type of the values that are returned from the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IAggregatingState<TInput, TOutput> GetAggregatingState<TInput, TAccumulator, TOutput>(AggregatingStateDescriptor<TInput, TAccumulator, TOutput> stateProperties);

        /// <summary>
        /// Gets a handle to the system's key/value map state. This state is similar to the state accessed via <see cref="GetState{TValue}"/>, but is optimized for state that is composed of user-defined key-value pairs.
        /// This state is only accessible if the function is executed on a KeyedStream.
        /// </summary>
        /// <typeparam name="TKey">The type of the user keys stored in the state.</typeparam>
        /// <typeparam name="TValue">The type of the user values stored in the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IMapState<TKey, TValue> GetMapState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateProperties);
    }
}
