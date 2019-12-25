using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    /// <summary>
    /// A serializer for long arrays.
    /// </summary>
    public class LongPrimitiveArraySerializer : TypeSerializerSingleton<long[]>
    {
        private static readonly long[] Empty = new long[0];

        public static readonly LongPrimitiveArraySerializer Instance = new LongPrimitiveArraySerializer();

        public override bool IsImmutableType => false;

        public override long[] CreateInstance() => Empty;

        public override long[] Copy(long[] @from)
        {
            var length = from.Length;
            var copy = new long[length];

            System.Array.Copy(from, 0, copy, 0, length);

            return copy;
        }

        public override long[] Copy(long[] @from, long[] reuse) => Copy(from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override int Length => -1;
        public override void Serialize(long[] record, IDataOutputView target)
        {
            if (record == null)
            {
                throw new IllegalArgumentException("The record must not be null.");
            }

            var len = record.Length;
            target.WriteLong(len);
            for (var i = 0; i < len; i++)
            {
                target.WriteLong(record[i]);
            }
        }

        public override long[] Deserialize(IDataInputView source)
        {
            var len = source.ReadLong();
            var result = new long[len];

            for (var i = 0; i < len; i++)
            {
                result[i] = source.ReadLong();
            }
            return result;
        }

        public override long[] Deserialize(long[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<long[]> SnapshotConfiguration() => new LongPrimitiveArraySerializerSnapshot();

        public class LongPrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<long[]>
        {
            public LongPrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
