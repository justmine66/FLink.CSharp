using System.Collections.Generic;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="IState"/> interface for partitioned key-value state. The key-value pair can be added, updated and retrieved.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IMapState<TKey, TValue> : IState, IEnumerable<TValue>
    {
        TValue this[TKey key] { get; }

        void Get(TKey key);

        void Put(TKey key, TValue value);

        void PutAll(Dictionary<TKey, TValue> map);

        void Remove(TKey key);

        bool Contains(TKey key);

        IEnumerable<KeyValuePair<TKey, TValue>> Entries { get; }

        IEnumerable<TKey> Keys { get; }


        IEnumerable<TValue> Values { get; }
    }
}
