using System;
using System.Linq;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class BoolPrimitiveArrayComparator : PrimitiveArrayComparator<bool, BoolComparator>
    {
        public BoolPrimitiveArrayComparator(bool @ascending)
            : base(@ascending, new BoolComparator(@ascending))
        { }

        public override int Hash(bool[] record) => record.Sum(field => field ? 1231 : 1237);

        public override int Compare(bool[] first, bool[] second)
        {
            for (var x = 0; x < Math.Min(first.Length, second.Length); x++)
            {
                var result = (second[x] == first[x] ? 0 : (first[x] ? 1 : -1));
                if (result != 0)
                {
                    return Ascending ? result : -result;
                }
            }

            var cmp = first.Length - second.Length;

            return Ascending ? cmp : -cmp;
        }

        public override TypeComparator<bool[]> Duplicate()
        {
            var dupe = new BoolPrimitiveArrayComparator(Ascending);
            dupe.SetReference(Reference);
            return dupe;
        }
    }
}
