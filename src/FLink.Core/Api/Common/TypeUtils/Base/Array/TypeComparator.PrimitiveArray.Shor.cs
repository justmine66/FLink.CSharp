using System;
using System.Linq;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class ShortPrimitiveArrayComparator : PrimitiveArrayComparator<short, BasicTypeComparator<short>>
    {
        public static readonly ShortPrimitiveArrayComparator Instance = new ShortPrimitiveArrayComparator();

        public ShortPrimitiveArrayComparator(bool @ascending = true)
            : base(@ascending, new ShortComparator(ascending))
        { }

        public override int Hash(short[] record) => record.Sum(field => field);

        public override int Compare(short[] first, short[] second)
        {
            for (var x = 0; x < Math.Min(first.Length, second.Length); x++)
            {
                var result = first[x] - second[x];
                if (result != 0)
                {
                    return Ascending ? result : -result;
                }
            }

            var cmp = first.Length - second.Length;
            return Ascending ? cmp : -cmp;
        }

        public override TypeComparator<short[]> Duplicate()
        {
            var dupe = new ShortPrimitiveArrayComparator(Ascending);
            dupe.SetReference(Reference);
            return dupe;
        }
    }
}
