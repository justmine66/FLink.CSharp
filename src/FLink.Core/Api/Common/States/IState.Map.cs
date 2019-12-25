using System.Collections.Generic;

namespace FLink.Core.Api.Common.States
{
    /// <summary>
    /// <see cref="IState"/> interface for partitioned key-value state. The key-value pair can be added, updated and retrieved.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys in the state.</typeparam>
    /// <typeparam name="TValue">Type of the values in the state.</typeparam>
    public interface IMapState<TKey, TValue> : IState, IEnumerable<TValue>
    {
        /// <summary>
        /// Returns the current value associated with the given key.
        /// </summary>
        /// <param name="key">The key of the mapping</param>
        /// <returns>The value of the mapping with the given key</returns>
        /// <exception cref="System.Exception">Thrown if the system cannot access the state.</exception>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Returns the current value associated with the given key.
        /// </summary>
        /// <param name="key">The key of the mapping</param>
        /// <returns>The value of the mapping with the given key</returns>
        /// <exception cref="System.Exception">Thrown if the system cannot access the state.</exception>
        TValue Get(TKey key);

        /// <summary>
        /// Associates a new value with the given key.
        /// </summary>
        /// <param name="key">The key of the mapping</param>
        /// <param name="value">The new value of the mapping</param>
        /// <exception cref="System.Exception">Thrown if the system cannot access the state.</exception>
        void Put(TKey key, TValue value);

        /// <summary>
        /// Copies all of the mappings from the given map into the state.
        /// </summary>
        /// <param name="map">The mappings to be stored in this state</param>
        /// <exception cref="System.Exception">Thrown if the system cannot access the state.</exception>
        void PutAll(Dictionary<TKey, TValue> map);

        /// <summary>
        /// Deletes the mapping of the given key.
        /// </summary>
        /// <param name="key">The key of the mapping</param>
        /// <exception cref="System.Exception">Thrown if the system cannot access the state.</exception>
        void Remove(TKey key);

        /// <summary>
        /// Returns whether there exists the given mapping.
        /// </summary>
        /// <param name="key">The key of the mapping</param>
        /// <returns>True if there exists a mapping whose key equals to the given key</returns>
        /// <exception cref="System.Exception">Thrown if the system cannot access the state.</exception>
        bool Contains(TKey key);

        /// <summary>
        /// Returns all the mappings in the state.
        /// An iterable view of all the key-value pairs in the state.
        /// </summary>
        /// <exception cref="System.Exception">Thrown if the system cannot access the state.</exception>
        IEnumerable<KeyValuePair<TKey, TValue>> Entries { get; }

        /// <summary>
        /// Returns all the keys in the state.
        /// An iterable view of all the keys in the state.
        /// </summary>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Returns all the values in the state.
        /// An iterable view of all the values in the state.
        /// </summary>
        /// <exception cref="System.Exception">Thrown if the system cannot access the state.</exception>
        IEnumerable<TValue> Values { get; }
    }
}
