using System;
using System.Linq;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class LongPrimitiveArrayComparator : PrimitiveArrayComparator<long, LongComparator>
    {
        public LongPrimitiveArrayComparator(bool @ascending)
            : base(@ascending, new LongComparator(ascending))
        { }

        public override int Hash(long[] record) => record.Sum(field => (int)field);

        public override int Compare(long[] first, long[] second)
        {
            for (var x = 0; x < Math.Min(first.Length, second.Length); x++)
            {
                var result = first[x].CompareTo(second[x]);
                if (result != 0)
                {
                    return Ascending ? result : -result;
                }
            }

            var cmp = first.Length - second.Length;
            return Ascending ? cmp : -cmp;
        }

        public override TypeComparator<long[]> Duplicate()
        {
            var dupe = new LongPrimitiveArrayComparator(Ascending);
            dupe.SetReference(Reference);
            return dupe;
        }
    }
}
