using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class IntSerializer : TypeSerializerSingleton<int>
    {
        // Sharable instance of the IntSerializer.
        public static readonly IntSerializer Instance = new IntSerializer();

        public static readonly int Zero = 0;

        public override bool IsImmutableType => true;

        public override int CreateInstance() => Zero;

        public override int Copy(int @from) => @from;

        public override int Copy(int @from, int reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target) => target.WriteInt(source.ReadInt());

        public override int Length => 4;

        public override void Serialize(int record, IDataOutputView target) => target.WriteInt(record);

        public override int Deserialize(IDataInputView source) => source.ReadInt();

        public override int Deserialize(int reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<int> SnapshotConfiguration() => new IntSerializerSnapshot();

        public class IntSerializerSnapshot : SimpleTypeSerializerSnapshot<int>
        {
            public IntSerializerSnapshot() : base(() => Instance)
            {
            }
        }
    }
}
