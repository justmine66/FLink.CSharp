using FLink.Core.Memory;
using System;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class DoubleComparator : BasicTypeComparator<double>
    {
        public static DoubleComparator Instance = new DoubleComparator();

        public DoubleComparator(bool @ascending = false)
            : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var l1 = firstSource.ReadDouble();
            var l2 = secondSource.ReadDouble();
            var comp = (l1 < l2 ? -1 : (l1 > l2 ? 1 : 0));

            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => false;

        public override int NormalizeKeyLength => 0;

        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => true;

        public override void PutNormalizedKey(double record, MemorySegment target, int offset, int numBytes) => throw new InvalidOperationException();

        public override TypeComparator<double> Duplicate() => new DoubleComparator(AscendingComparison);
    }
}
