using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    /// <summary>
    /// A serializer for byte arrays.
    /// </summary>
    public class BytePrimitiveArraySerializer : TypeSerializerSingleton<byte[]>
    {
        private static readonly byte[] Empty = new byte[0];

        public static readonly BytePrimitiveArraySerializer Instance = new BytePrimitiveArraySerializer();

        public override bool IsImmutableType => false;

        public override byte[] CreateInstance() => Empty;

        public override byte[] Copy(byte[] @from)
        {
            var length = from.Length;
            var copy = new byte[length];

            System.Array.Copy(from, 0, copy, 0, length);

            return copy;
        }

        public override byte[] Copy(byte[] @from, byte[] reuse) => Copy(from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override int Length => -1;
        public override void Serialize(byte[] record, IDataOutputView target)
        {
            if (record == null)
            {
                throw new IllegalArgumentException("The record must not be null.");
            }

            var len = record.Length;
            target.WriteInt(len);
            target.Write(record);
        }

        public override byte[] Deserialize(IDataInputView source)
        {
            var len = source.ReadInt();
            var result = new byte[len];
            source.ReadFully(result);
            return result;
        }

        public override byte[] Deserialize(byte[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<byte[]> SnapshotConfiguration() => new BytePrimitiveArraySerializerSnapshot();

        public class BytePrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<byte[]>
        {
            public BytePrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
