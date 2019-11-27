using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class IntComparator : BasicTypeComparator<int>
    {
        public static IntComparator Instance = new IntComparator();

        public IntComparator(bool @ascending = false)
            : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var i1 = firstSource.ReadInt();
            var i2 = secondSource.ReadInt();
            var comp = (i1 < i2 ? -1 : (i1 == i2 ? 0 : 1));
            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => true;
        public override int NormalizeKeyLength => 4;
        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => keyBytes < 4;

        public override void PutNormalizedKey(int record, MemorySegment target, int offset, int numBytes)
        {
            throw new System.NotImplementedException();
        }

        public override TypeComparator<int> Duplicate() => new IntComparator(AscendingComparison);
    }
}
