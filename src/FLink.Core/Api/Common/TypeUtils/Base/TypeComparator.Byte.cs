using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class ByteComparator : BasicTypeComparator<byte>
    {
        public static ByteComparator Instance = new ByteComparator();

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var b1 = firstSource.ReadByte();
            var b2 = secondSource.ReadByte();
            var comp = (b1 < b2 ? -1 : (b1 == b2 ? 0 : 1));
            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => true;
        public override int NormalizeKeyLength => 1;
        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => keyBytes < 1;

        public override void PutNormalizedKey(byte record, MemorySegment target, int offset, int numBytes)
        {
            throw new NotImplementedException();
        }

        public override TypeComparator<byte> Duplicate() => new ByteComparator(AscendingComparison);

        public ByteComparator(bool @ascending = false)
            : base(@ascending)
        {
        }
    }
}
