using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class BoolPrimitiveArraySerializer : TypeSerializerSingleton<bool[]>
    {
        private static readonly bool[] Empty = new bool[0];
        public static readonly BoolPrimitiveArraySerializer Instance = new BoolPrimitiveArraySerializer();

        public override bool IsImmutableType => false;
        public override bool[] CreateInstance() => Empty;

        public override bool[] Copy(bool[] @from)
        {
            var length = from.Length;
            var copy = new bool[length];
            System.Array.Copy(@from, 0, copy, 0, length);
            return copy;
        }

        public override bool[] Copy(bool[] @from, bool[] reuse) => Copy(@from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override int Length => -1;
        public override void Serialize(bool[] record, IDataOutputView target)
        {
            if (record == null)
            {
                throw new IllegalArgumentException("The record must not be null.");
            }

            var len = record.Length;
            target.WriteInt(len);
            for (var i = 0; i < len; i++)
            {
                target.WriteBool(record[i]);
            }
        }

        public override bool[] Deserialize(IDataInputView source)
        {
            var len = source.ReadInt();
            var result = new bool[len];

            for (var i = 0; i < len; i++)
            {
                result[i] = source.ReadBool();
            }

            return result;
        }

        public override bool[] Deserialize(bool[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<bool[]> SnapshotConfiguration() => new BoolPrimitiveArraySerializerSnapshot();

        public class BoolPrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<bool[]>
        {
            public BoolPrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
