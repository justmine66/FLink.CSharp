using FLink.Core.Memory;
using System;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class FloatSerializer : TypeSerializerSingleton<float>
    {
        public static readonly FloatSerializer Instance = new FloatSerializer();
        public const float Zero = 0f;

        public override bool IsImmutableType => true;

        public override float CreateInstance() => Zero;

        public override float Copy(float @from) => @from;

        public override float Copy(float @from, float reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target) => target.WriteFloat(source.ReadFloat());

        public override int Length => sizeof(float);

        public override void Serialize(float record, IDataOutputView target) => target.WriteFloat(record);

        public override float Deserialize(IDataInputView source) => source.ReadFloat();

        public override float Deserialize(float reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<float> SnapshotConfiguration() => new FloatSerializerSnapshot();

        public sealed class FloatSerializerSnapshot : SimpleTypeSerializerSnapshot<float>
        {
            public FloatSerializerSnapshot() : base(() => Instance)
            {
            }
        }
    }
}
