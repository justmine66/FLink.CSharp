using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class ShortSerializer : TypeSerializerSingleton<short>
    {
        public static readonly ShortSerializer Instance = new ShortSerializer();

        private const short Zero = 0;

        public override bool IsImmutableType => true;

        public override short CreateInstance() => Zero;

        public override short Copy(short @from) => @from;

        public override short Copy(short @from, short reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target) => target.WriteShort(source.ReadShort());

        public override int Length => sizeof(short);

        public override void Serialize(short record, IDataOutputView target) => target.WriteShort(record);

        public override short Deserialize(IDataInputView source) => source.ReadShort();

        public override short Deserialize(short reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<short> SnapshotConfiguration() => new ShortSerializerSnapshot();

        public sealed class ShortSerializerSnapshot : SimpleTypeSerializerSnapshot<short>
        {
            public ShortSerializerSnapshot() : base(() => Instance)
            {
            }
        }
    }
}
