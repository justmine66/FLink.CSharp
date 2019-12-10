using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class DecimalComparator : BasicTypeComparator<decimal>
    {
        public static DecimalComparator Instance = new DecimalComparator();

        public DecimalComparator(bool @ascending = false) : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            throw new NotImplementedException();
        }

        public override bool SupportsNormalizedKey { get; }
        public override int NormalizeKeyLength { get; }
        public override bool IsNormalizedKeyPrefixOnly(int keyBytes)
        {
            throw new NotImplementedException();
        }

        public override void PutNormalizedKey(decimal record, MemorySegment target, int offset, int numBytes)
        {
            throw new NotImplementedException();
        }

        public override TypeComparator<decimal> Duplicate()
        {
            throw new NotImplementedException();
        }
    }
}
