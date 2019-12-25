using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.States;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Util;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    /// <summary>
    /// Special <see cref="TypeInformation{TType}"/> used by <see cref="MapStateDescriptor{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    public class MapTypeInfo<TKey, TValue> : TypeInformation<IDictionary<TKey, TValue>>
    {
        public MapTypeInfo(TypeInformation<TKey> keyTypeInfo, TypeInformation<TValue> valueTypeInfo)
        {
            KeyTypeInfo = Preconditions.CheckNotNull(keyTypeInfo, "The key type information cannot be null."); ;
            ValueTypeInfo = Preconditions.CheckNotNull(valueTypeInfo, "The value type information cannot be null.");
        }

        /// <summary>
        /// Gets the type information for the keys in the map
        /// </summary>
        public TypeInformation<TKey> KeyTypeInfo { get; }

        /// <summary>
        /// Gets the type information for the values in the map
        /// </summary>
        public TypeInformation<TValue> ValueTypeInfo { get; }

        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => 0;
        public override int TotalFields => 1;
        public override Type TypeClass => typeof(IDictionary<TKey, TValue>);
        public override bool IsKeyType => false;
        public override TypeSerializer<IDictionary<TKey, TValue>> CreateSerializer(ExecutionConfig config)
        {
            var keyTypeSerializer = KeyTypeInfo.CreateSerializer(config);
            var valueTypeSerializer = ValueTypeInfo.CreateSerializer(config);

            return new MapSerializer<TKey, TValue>(keyTypeSerializer, valueTypeSerializer);
        }

        public override string ToString() => $"Map<{KeyTypeInfo}, {ValueTypeInfo}>";

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
