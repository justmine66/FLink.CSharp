using FLink.Core.Memory;
using System;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class CharComparator : BasicTypeComparator<char>
    {
        public static CharComparator Instance = new CharComparator();

        public const byte Size = sizeof(char);

        public CharComparator(bool @ascending = false)
            : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var c1 = firstSource.ReadChar();
            var c2 = secondSource.ReadChar();
            var comp = (c1 < c2 ? -1 : (c1 == c2 ? 0 : 1));

            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => true;

        public override int NormalizeKeyLength => Size;

        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => keyBytes < 2;

        public override void PutNormalizedKey(char record, MemorySegment target, int offset, int numBytes)
        {
            throw new NotImplementedException();
        }

        public override TypeComparator<char> Duplicate() => new CharComparator(AscendingComparison);
    }
}
