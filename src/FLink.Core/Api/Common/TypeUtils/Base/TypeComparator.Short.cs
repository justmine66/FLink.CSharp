using FLink.Core.Memory;
using System;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class ShortComparator : BasicTypeComparator<short>
    {
        public static ShortComparator Instance = new ShortComparator();

        public const byte Size = sizeof(short);

        public ShortComparator(bool @ascending = false)
            : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var s1 = firstSource.ReadShort();
            var s2 = secondSource.ReadShort();
            var comp = (s1 < s2 ? -1 : (s1 == s2 ? 0 : 1));

            return AscendingComparison ? comp : -comp;
        }

        public override bool SupportsNormalizedKey => true;

        public override int NormalizeKeyLength => Size;

        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => keyBytes < Size;

        public override void PutNormalizedKey(short record, MemorySegment target, int offset, int numBytes)
        {
            throw new NotImplementedException();
        }

        public override TypeComparator<short> Duplicate() => new ShortComparator(AscendingComparison);
    }
}
