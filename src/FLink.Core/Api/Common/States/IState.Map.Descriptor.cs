using System.Collections.Generic;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Api.CSharp.TypeUtils;

namespace FLink.Core.Api.Common.States
{
    /// <summary>
    /// A <see cref="StateDescriptor{TState, T}"/> for <see cref="IMapState{TKey, TValue}"/>. This can be used to create state where the type is a map that can be updated and iterated over.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys that can be added to the map state.</typeparam>
    /// <typeparam name="TValue">The type of the values that can be added to the map state.</typeparam>
    public class MapStateDescriptor<TKey, TValue> : StateDescriptor<IMapState<TKey, TValue>, IDictionary<TKey, TValue>>
    {
        public MapStateDescriptor(
            string name,
            TypeSerializer<TKey> keySerializer,
            TypeSerializer<TValue> valueSerializer,
            IDictionary<TKey, TValue> defaultValue = default)
            : base(name, new MapSerializer<TKey, TValue>(keySerializer, valueSerializer), defaultValue)
        {
        }

        public MapStateDescriptor(
            string name,
            TypeInformation<TKey> keyTypeInfo,
            TypeInformation<TValue> valueTypeInfo,
            IDictionary<TKey, TValue> defaultValue = default)
            : base(name, new MapTypeInfo<TKey, TValue>(keyTypeInfo, valueTypeInfo), defaultValue)
        {
        }

        public MapStateDescriptor(
            string name,
            TypeSerializer<IDictionary<TKey, TValue>> serializer,
            IDictionary<TKey, TValue> defaultValue = default)
            : base(name, serializer, defaultValue)
        {
        }

        public override StateType Type => StateType.Map;
    }
}
