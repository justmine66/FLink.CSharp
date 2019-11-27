using FLink.Core.Memory;
using System;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class DateTimeComparator : BasicTypeComparator<DateTime>
    {
        public static DateTimeComparator Instance = new DateTimeComparator();

        public const byte Size = sizeof(long);

        public DateTimeComparator(bool @ascending = false)
            : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource) =>
            CompareSerializedDate(firstSource, secondSource, AscendingComparison);

        public override bool SupportsNormalizedKey => true;

        public override int NormalizeKeyLength => Size;

        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => keyBytes < Size;

        public override void PutNormalizedKey(DateTime record, MemorySegment target, int offset, int numBytes)
        {
            throw new NotImplementedException();
        }

        public override TypeComparator<DateTime> Duplicate() => new DateTimeComparator(AscendingComparison);

        #region [ Static Helpers for Date Comparison ]

        public static int CompareSerializedDate(IDataInputView firstSource, IDataInputView secondSource, bool ascendingComparison)
        {
            var l1 = firstSource.ReadLong();
            var l2 = secondSource.ReadLong();
            var comp = (l1 < l2 ? -1 : (l1 == l2 ? 0 : 1));

            return ascendingComparison ? comp : -comp;
        }

        #endregion
    }
}
