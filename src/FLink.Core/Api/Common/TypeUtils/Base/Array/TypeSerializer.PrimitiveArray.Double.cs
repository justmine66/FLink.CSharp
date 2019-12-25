using System;
using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class DoublePrimitiveArraySerializer : TypeSerializerSingleton<double[]>
    {
        private static readonly double[] Empty = new double[0];

        public static readonly DoublePrimitiveArraySerializer Instance = new DoublePrimitiveArraySerializer();

        public override bool IsImmutableType => false;

        public override double[] CreateInstance() => Empty;

        public override double[] Copy(double[] @from)
        {
            var length = from.Length;
            var copy = new double[length];
            System.Array.Copy(from, 0, copy, 0, length);
            return copy;
        }

        public override double[] Copy(double[] @from, double[] reuse) => Copy(from);

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override int Length => -1;

        public override void Serialize(double[] record, IDataOutputView target)
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

        public override double[] Deserialize(IDataInputView source)
        {
            var len = source.ReadInt();
            var result = new double[len];

            for (var i = 0; i < len; i++)
            {
                result[i] = source.ReadDouble();
            }

            return result;
        }

        public override double[] Deserialize(double[] reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<double[]> SnapshotConfiguration() => new DoublePrimitiveArraySerializerSnapshot();

        public class DoublePrimitiveArraySerializerSnapshot : SimpleTypeSerializerSnapshot<double[]>
        {
            public DoublePrimitiveArraySerializerSnapshot()
                : base(() => Instance)
            { }
        }
    }
}
