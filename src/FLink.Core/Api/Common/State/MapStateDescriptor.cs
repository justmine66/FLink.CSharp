using System.Collections.Generic;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// A <see cref="StateDescriptor{TState, T}"/> for <see cref="IMapState{TKey, TValue}"/>. This can be used to create state where the type is a map that can be updated and iterated over.
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public class MapStateDescriptor<TK, TV> : StateDescriptor<IMapState<TK, TV>, Dictionary<TK, TV>>
    {
        public MapStateDescriptor(string name, Dictionary<TK, TV> defaultValue = null)
            : base(name, defaultValue)
        {
        }
    }
}
