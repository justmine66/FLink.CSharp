using System;
using System.Collections.Generic;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class MapSerializer<TKey, TValue> : TypeSerializer<IDictionary<TKey, TValue>>
    {
        public MapSerializer(TypeSerializer<TKey> keySerializer, TypeSerializer<TValue> valueSerializer)
        {
            KeySerializer = keySerializer;
            ValueSerializer = valueSerializer;
        }

        public TypeSerializer<TKey> KeySerializer { get; }

        public TypeSerializer<TValue> ValueSerializer { get; }

        public override bool IsImmutableType => false;
        public override TypeSerializer<IDictionary<TKey, TValue>> Duplicate()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<TKey, TValue> CreateInstance()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<TKey, TValue> Copy(IDictionary<TKey, TValue> @from)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<TKey, TValue> Copy(IDictionary<TKey, TValue> @from, IDictionary<TKey, TValue> reuse)
        {
            throw new NotImplementedException();
        }

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override int Length => -1;
        public override void Serialize(IDictionary<TKey, TValue> record, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<TKey, TValue> Deserialize(IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<TKey, TValue> Deserialize(IDictionary<TKey, TValue> reuse, IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override ITypeSerializerSnapshot<IDictionary<TKey, TValue>> SnapshotConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
