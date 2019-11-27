using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class BoolComparator : BasicTypeComparator<bool>
    {
        public static readonly BoolComparator Instance = new BoolComparator();

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var fs = firstSource.ReadBool() ? 1 : 0;
            var ss = secondSource.ReadBool() ? 1 : 0;
            var comp = fs - ss;
            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => true;

        public override int NormalizeKeyLength => 1;

        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => keyBytes < 1;

        public override void PutNormalizedKey(bool record, MemorySegment target, int offset, int numBytes)
        {
            throw new NotImplementedException();
        }

        public override TypeComparator<bool> Duplicate() => new BoolComparator(AscendingComparison);

        public BoolComparator(bool @ascending = false)
            : base(@ascending)
        {
        }
    }
}
