using System.Collections.Generic;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// This interface contains methods for registering operator state with a managed store.
    /// </summary>
    public interface IOperatorStateStore
    {
        /// <summary>
        /// Creates (or restores) a <see cref="IBroadcastState{TKey,TValue}"/>. This type of state can only be created to store the state of a BroadcastStream. Each state is registered under a unique name.
        /// The provided serializer is used to de/serialize the state in case of checkpointing (snapshot/restore).
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="stateDescriptor"></param>
        /// <returns></returns>
        IBroadcastState<TKey, TValue> GetBroadcastState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateDescriptor);

        /// <summary>
        /// Creates (or restores) a list state. Each state is registered under a unique name.
        /// The provided serializer is used to de/serialize the state in case of checkpointing(snapshot/restore).
        /// <remarks>
        /// Note the semantic differences between an operator list state and a keyed list state.
        /// </remarks>
        /// </summary>
        /// <typeparam name="TValue">The generic type of the state</typeparam>
        /// <param name="stateDescriptor">The descriptor for this state, providing a name and serializer.</param>
        /// <returns>A list for all state partitions.</returns>
        IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateDescriptor);

        /// <summary>
        /// Returns a set with the names of all currently registered states.
        /// </summary>
        IEnumerable<string> RegisteredStateNames { get; }

        /// <summary>
        /// Returns a set with the names of all currently registered broadcast states.
        /// </summary>
        IEnumerable<string> RegisteredBroadcastStateNames { get; }
    }
}
