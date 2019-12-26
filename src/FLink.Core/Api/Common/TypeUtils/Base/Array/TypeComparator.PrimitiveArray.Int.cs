using System;
using System.Linq;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class IntPrimitiveArrayComparator : PrimitiveArrayComparator<int, BasicTypeComparator<int>>
    {
        public static readonly IntPrimitiveArrayComparator Instance = new IntPrimitiveArrayComparator();

        public IntPrimitiveArrayComparator(bool @ascending = true)
            : base(@ascending, new IntComparator(ascending))
        { }

        public override int Hash(int[] record) => record.Sum(field => (int)field);

        public override int Compare(int[] first, int[] second)
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

        public override TypeComparator<int[]> Duplicate()
        {
            var dupe = new IntPrimitiveArrayComparator(Ascending);
            dupe.SetReference(Reference);
            return dupe;
        }
    }
}
