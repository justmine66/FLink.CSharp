using System;
using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class StringPrimitiveArraySerializer : TypeSerializerSingleton<string[]>
    {
        private static readonly string[] Empty = new string[0];

        public static readonly StringPrimitiveArraySerializer Instance = new StringPrimitiveArraySerializer();

        public override bool IsImmutableType => false;

        public override string[] CreateInstance() => Empty;

        public override string[] Copy(string[] @from)
        {
            var length = from.Length;
            var copy = new string[length];
            System.Array.Copy(from, 0, copy, 0, length);
            return copy;
        }

        public override string[] Copy(string[] @from, string[] reuse) => Copy(from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override int Length => -1;

        public override void Serialize(string[] record, IDataOutputView target)
        {
            if (record == null)
            {
                throw new IllegalArgumentException("The record must not be null.");
            }

            var len = record.Length;
            target.WriteInt(len);
            for (var i = 0; i < len; i++)
            {
                target.WriteChars(record[i]);
            }
        }

        public override string[] Deserialize(IDataInputView source)
        {
            var len = source.ReadInt();
            var result = new string[len];

            for (var i = 0; i < len; i++)
            {
                result[i] = source.ReadLine();
            }

            return result;
        }

        public override string[] Deserialize(string[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<string[]> SnapshotConfiguration() => new StringPrimitiveArraySerializerSnapshot();

        public class StringPrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<string[]>
        {
            public StringPrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
