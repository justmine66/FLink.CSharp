using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class ByteSerializer : TypeSerializerSingleton<byte>
    {
        public static readonly ByteSerializer Instance = new ByteSerializer();

        private const byte Zero = (byte)0;

        public override bool IsImmutableType => true;

        public override byte CreateInstance() => Zero;

        public override byte Copy(byte @from) => @from;

        public override byte Copy(byte @from, byte reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target) => target.WriteByte(source.ReadByte());

        public override int Length => 1;

        public override void Serialize(byte record, IDataOutputView target) => target.WriteByte(record);

        public override byte Deserialize(IDataInputView source) => source.ReadByte();

        public override byte Deserialize(byte reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<byte> SnapshotConfiguration() => new ByteSerializerSnapshot();

        public sealed class ByteSerializerSnapshot : SimpleTypeSerializerSnapshot<byte>
        {
            public ByteSerializerSnapshot()
                : base(() => Instance)
            {
            }
        }
    }
}
