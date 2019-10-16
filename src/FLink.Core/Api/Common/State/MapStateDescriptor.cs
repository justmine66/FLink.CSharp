using System.Collections.Generic;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// A <see cref="StateDescriptor{TState, T}"/> for <see cref="IMapState{TKey, TValue}"/>. This can be used to create state where the type is a map that can be updated and iterated over.
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public class MapStateDescriptor<TK, TV> : StateDescriptor<IMapState<TK, TV>, Dictionary<TK, TV>>
    {
        public MapStateDescriptor(string name, TypeSerializer<Dictionary<TK, TV>> serializer, Dictionary<TK, TV> defaultValue = default) 
            : base(name, serializer, defaultValue)
        {
        }
    }
}
