using System;
using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class FloatPrimitiveArraySerializer : TypeSerializerSingleton<float[]>
    {
        private static readonly float[] Empty = new float[0];

        public static readonly FloatPrimitiveArraySerializer Instance = new FloatPrimitiveArraySerializer();

        public override bool IsImmutableType => false;

        public override float[] CreateInstance() => Empty;

        public override float[] Copy(float[] @from)
        {
            var length = from.Length;
            var copy = new float[length];
            System.Array.Copy(from, 0, copy, 0, length);
            return copy;
        }

        public override float[] Copy(float[] @from, float[] reuse) => Copy(from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override int Length => -1;

        public override void Serialize(float[] record, IDataOutputView target)
        {
            if (record == null)
            {
                throw new IllegalArgumentException("The record must not be null.");
            }

            var len = record.Length;
            target.WriteInt(len);
            for (var i = 0; i < len; i++)
            {
                target.WriteDouble(record[i]);
            }
        }

        public override float[] Deserialize(IDataInputView source)
        {
            var len = source.ReadInt();
            var result = new float[len];

            for (var i = 0; i < len; i++)
            {
                result[i] = source.ReadFloat();
            }

            return result;
        }

        public override float[] Deserialize(float[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<float[]> SnapshotConfiguration() => new FloatPrimitiveArraySerializerSnapshot();

        public class FloatPrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<float[]>
        {
            public FloatPrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
