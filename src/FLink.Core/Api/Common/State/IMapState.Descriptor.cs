using System.Collections.Generic;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// A <see cref="StateDescriptor{TState, T}"/> for <see cref="IMapState{TKey, TValue}"/>. This can be used to create state where the type is a map that can be updated and iterated over.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys that can be added to the map state.</typeparam>
    /// <typeparam name="TValue">The type of the values that can be added to the map state.</typeparam>
    public class MapStateDescriptor<TKey, TValue> : StateDescriptor<IMapState<TKey, TValue>, Dictionary<TKey, TValue>>
    {
        public MapStateDescriptor(string name, TypeSerializer<Dictionary<TKey, TValue>> serializer, Dictionary<TKey, TValue> defaultValue = default)
            : base(name, serializer, defaultValue)
        {
        }
    }
}
