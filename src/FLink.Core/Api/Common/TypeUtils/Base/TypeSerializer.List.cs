using System;
using System.Collections.Generic;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class ListSerializer<T> : TypeSerializer<IList<T>>
    {
        public ListSerializer(TypeSerializer<T> elementSerializer)
        {
            ElementSerializer = elementSerializer;
        }

        /// <summary>
        /// Gets the serializer for the elements of the list.
        /// </summary>
        public TypeSerializer<T> ElementSerializer { get; }
        public override bool IsImmutableType { get; }
        public override TypeSerializer<IList<T>> Duplicate()
        {
            throw new NotImplementedException();
        }

        public override IList<T> CreateInstance()
        {
            throw new NotImplementedException();
        }

        public override IList<T> Copy(IList<T> @from)
        {
            throw new NotImplementedException();
        }

        public override IList<T> Copy(IList<T> @from, IList<T> reuse)
        {
            throw new NotImplementedException();
        }

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override int Length { get; }
        public override void Serialize(IList<T> record, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override IList<T> Deserialize(IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override IList<T> Deserialize(IList<T> reuse, IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override ITypeSerializerSnapshot<IList<T>> SnapshotConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
