using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class LongComparator : BasicTypeComparator<long>
    {
        public const byte Size = sizeof(long);

        public static LongComparator Instance = new LongComparator();

        public LongComparator(bool @ascending = false)
            : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var l1 = firstSource.ReadLong();
            var l2 = secondSource.ReadLong();
            var comp = (l1 < l2 ? -1 : (l1 == l2 ? 0 : 1));
            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => true;
        public override int NormalizeKeyLength => Size;
        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => keyBytes < Size;

        public override void PutNormalizedKey(long record, MemorySegment target, int offset, int numBytes)
        {
            throw new NotImplementedException();
        }

        public override TypeComparator<long> Duplicate() => new LongComparator(AscendingComparison);
    }
}
