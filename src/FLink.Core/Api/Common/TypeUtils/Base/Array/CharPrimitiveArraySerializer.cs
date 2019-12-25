using System;
using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class CharPrimitiveArraySerializer : TypeSerializerSingleton<char[]>
    {
        private static readonly char[] Empty = new char[0];

        public static readonly CharPrimitiveArraySerializer Instance = new CharPrimitiveArraySerializer();

        public override bool IsImmutableType => false;

        public override char[] CreateInstance() => Empty;

        public override char[] Copy(char[] @from)
        {
            var length = from.Length;
            var copy = new char[length];
            System.Array.Copy(from, 0, copy, 0, length);
            return copy;
        }

        public override char[] Copy(char[] @from, char[] reuse) => Copy(from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override int Length => -1;

        public override void Serialize(char[] record, IDataOutputView target)
        {
            if (record == null)
            {
                throw new IllegalArgumentException("The record must not be null.");
            }

            var len = record.Length;
            target.WriteInt(len);
            for (var i = 0; i < len; i++)
            {
                target.WriteChar(record[i]);
            }
        }

        public override char[] Deserialize(IDataInputView source)
        {
            var len = source.ReadInt();
            var result = new char[len];

            for (var i = 0; i < len; i++)
            {
                result[i] = source.ReadChar();
            }

            return result;
        }

        public override char[] Deserialize(char[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<char[]> SnapshotConfiguration() => new CharPrimitiveArraySerializerSnapshot();

        public class CharPrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<char[]>
        {
            public CharPrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
