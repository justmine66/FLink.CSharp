using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class DecimalSerializer : TypeSerializerSingleton<decimal>
    {
        public static readonly DecimalSerializer Instance = new DecimalSerializer();

        public override bool IsImmutableType => true;

        public override decimal CreateInstance() => decimal.Zero;

        public override decimal Copy(decimal @from) => from;

        public override decimal Copy(decimal @from, decimal reuse) => from;

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override int Length => sizeof(decimal);
        public override void Serialize(decimal record, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override decimal Deserialize(IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override decimal Deserialize(decimal reuse, IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override ITypeSerializerSnapshot<decimal> SnapshotConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
