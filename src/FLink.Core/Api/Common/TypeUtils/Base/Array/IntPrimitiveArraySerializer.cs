using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    /// <summary>
    /// A serializer for int arrays.
    /// </summary>
    public class IntPrimitiveArraySerializer : TypeSerializerSingleton<int[]>
    {
        private static readonly int[] Empty = new int[0];

        public static readonly IntPrimitiveArraySerializer Instance = new IntPrimitiveArraySerializer();

        public override bool IsImmutableType => false;

        public override int[] CreateInstance() => Empty;

        public override int[] Copy(int[] @from)
        {
            var length = from.Length;
            var copy = new int[length];

            System.Array.Copy(from, 0, copy, 0, length);

            return copy;
        }

        public override int[] Copy(int[] @from, int[] reuse) => Copy(from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override int Length => -1;
        public override void Serialize(int[] record, IDataOutputView target)
        {
            if (record == null)
            {
                throw new IllegalArgumentException("The record must not be null.");
            }

            var len = record.Length;
            target.WriteInt(len);
            for (var i = 0; i < len; i++)
            {
                target.WriteInt(record[i]);
            }
        }

        public override int[] Deserialize(IDataInputView source)
        {
            var len = source.ReadInt();
            var result = new int[len];

            for (var i = 0; i < len; i++)
            {
                result[i] = source.ReadInt();
            }
            return result;
        }

        public override int[] Deserialize(int[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<int[]> SnapshotConfiguration() => new IntPrimitiveArraySerializerSnapshot();

        public class IntPrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<int[]>
        {
            public IntPrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
