using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class BoolComparator : BasicTypeComparator<bool>
    {
        public const byte Size = sizeof(bool);

        public static readonly BoolComparator Instance = new BoolComparator();

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var fs = firstSource.ReadBool() ? 1 : 0;
            var ss = secondSource.ReadBool() ? 1 : 0;
            var comp = fs - ss;
            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => true;

        public override int NormalizeKeyLength => Size;

        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => keyBytes < Size;

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
