using FLink.Core.Memory;
using System;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class DoubleSerializer : TypeSerializerSingleton<double>
    {
        public static readonly DoubleSerializer Instance = new DoubleSerializer();

        private const double Zero = 0.0;

        public override bool IsImmutableType => true;

        public override double CreateInstance() => Zero;

        public override double Copy(double @from) => @from;

        public override double Copy(double @from, double reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target) => target.WriteDouble(source.ReadDouble());

        public override int Length => sizeof(double);

        public override void Serialize(double record, IDataOutputView target) => target.WriteDouble(record);

        public override double Deserialize(IDataInputView source) => source.ReadDouble();

        public override double Deserialize(double reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<double> SnapshotConfiguration() => new DoubleSerializerSnapshot();

        public sealed class DoubleSerializerSnapshot : SimpleTypeSerializerSnapshot<double>
        {
            public DoubleSerializerSnapshot()
                : base(() => Instance)
            {
            }
        }
    }
}
