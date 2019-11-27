using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class LongSerializer : TypeSerializerSingleton<long>
    {
        public static readonly LongSerializer Instance = new LongSerializer();
        public const long Zero = 0L;

        public override bool IsImmutableType => true;

        public override long CreateInstance() => Zero;

        public override long Copy(long @from) => @from;

        public override long Copy(long @from, long reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target) => target.WriteLong(source.ReadLong());

        public override int Length => sizeof(long);

        public override void Serialize(long record, IDataOutputView target) => target.WriteLong(record);

        public override long Deserialize(IDataInputView source) => source.ReadLong();

        public override long Deserialize(long reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<long> SnapshotConfiguration() => new LongSerializerSnapshot();

        public class LongSerializerSnapshot : SimpleTypeSerializerSnapshot<long>
        {
            public LongSerializerSnapshot() : base(() => Instance)
            {
            }
        }
    }
}
