using FLink.Core.Memory;
using System;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class DateTimeSerializer : TypeSerializerSingleton<DateTime>
    {
        public static readonly DateTimeSerializer Instance = new DateTimeSerializer();

        public override bool IsImmutableType => false;

        public override DateTime CreateInstance() => new DateTime();

        public override DateTime Copy(DateTime @from) => new DateTime(@from.Ticks);

        public override DateTime Copy(DateTime @from, DateTime reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target) => target.WriteLong(source.ReadLong());

        public override int Length => sizeof(long);

        public override void Serialize(DateTime record, IDataOutputView target) => target.WriteLong(record.Ticks);

        public override DateTime Deserialize(IDataInputView source)
        {
            var ticks = source.ReadLong();

            return new DateTime(ticks);
        }

        public override DateTime Deserialize(DateTime reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<DateTime> SnapshotConfiguration() => new DateTimeSerializerSnapshot();

        public sealed class DateTimeSerializerSnapshot : SimpleTypeSerializerSnapshot<DateTime>
        {
            public DateTimeSerializerSnapshot() : base(() => Instance)
            {
            }
        }
    }
}
