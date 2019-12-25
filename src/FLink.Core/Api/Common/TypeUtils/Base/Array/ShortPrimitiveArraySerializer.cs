using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    /// <summary>
    /// A serializer for short arrays.
    /// </summary>
    public class ShortPrimitiveArraySerializer : TypeSerializerSingleton<short[]>
    {
        private static readonly short[] Empty = new short[0];

        public static readonly ShortPrimitiveArraySerializer Instance = new ShortPrimitiveArraySerializer();

        public override bool IsImmutableType => false;

        public override short[] CreateInstance() => Empty;

        public override short[] Copy(short[] @from)
        {
            var length = from.Length;
            var copy = new short[length];

            System.Array.Copy(from, 0, copy, 0, length);

            return copy;
        }

        public override short[] Copy(short[] @from, short[] reuse) => Copy(from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override int Length => -1;
        public override void Serialize(short[] record, IDataOutputView target)
        {
            if (record == null)
            {
                throw new IllegalArgumentException("The record must not be null.");
            }

            var len = record.Length;
            target.WriteShort(len);
            for (var i = 0; i < len; i++)
            {
                target.WriteShort(record[i]);
            }
        }

        public override short[] Deserialize(IDataInputView source)
        {
            var len = source.ReadShort();
            var result = new short[len];

            for (var i = 0; i < len; i++)
            {
                result[i] = source.ReadShort();
            }
            return result;
        }

        public override short[] Deserialize(short[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<short[]> SnapshotConfiguration() => new ShortPrimitiveArraySerializerSnapshot();

        public class ShortPrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<short[]>
        {
            public ShortPrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
