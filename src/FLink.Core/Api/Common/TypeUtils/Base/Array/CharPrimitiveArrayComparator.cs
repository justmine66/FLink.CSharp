using System;
using System.Linq;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class CharPrimitiveArrayComparator : PrimitiveArrayComparator<char, CharComparator>
    {
        public CharPrimitiveArrayComparator(bool @ascending)
            : base(@ascending, new CharComparator(ascending))
        { }

        public override int Hash(char[] record) => record.Sum(field => (int)field);

        public override int Compare(char[] first, char[] second)
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

        public override TypeComparator<char[]> Duplicate()
        {
            var dupe = new CharPrimitiveArrayComparator(Ascending);
            dupe.SetReference(Reference);
            return dupe;
        }
    }
}
