using System.Collections.Generic;

namespace FLink.Core.Api.Common.States
{
    /// <summary>
    /// A read-only view of the <see cref="IBroadcastState{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The key type of the elements in the <see cref="IReadOnlyBroadcastState{TKey, TValue}"/></typeparam>
    /// <typeparam name="TValue">The value type of the elements in the <see cref="IReadOnlyBroadcastState{TKey, TValue}"/></typeparam>
    public interface IReadOnlyBroadcastState<TKey, TValue> : IState
    {
        TValue this[TKey key] { get; }

        TValue Get(TKey key);

        bool Contains(TKey key);

        /// <summary>
        /// Returns an immutable entries in the state.
        /// The user code must not modify the entries of the returned immutable iterator, as this can lead to inconsistent states.
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<TKey, TValue>> ImmutableEntries();
    }
}
