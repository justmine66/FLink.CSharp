using FLink.Core.Memory;
using System;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class FloatComparator : BasicTypeComparator<float>
    {
        public static FloatComparator Instance = new FloatComparator();

        public FloatComparator(bool @ascending = false)
            : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var l1 = firstSource.ReadFloat();
            var l2 = secondSource.ReadFloat();
            var comp = (l1 < l2 ? -1 : (l1 > l2 ? 1 : 0));

            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => false;

        public override int NormalizeKeyLength => 0;

        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => true;

        public override void PutNormalizedKey(float record, MemorySegment target, int offset, int numBytes) => throw new InvalidOperationException();

        public override TypeComparator<float> Duplicate() => new FloatComparator(AscendingComparison);
    }
}
