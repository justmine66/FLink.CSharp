using System;
using System.Linq;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class BytePrimitiveArrayComparator : PrimitiveArrayComparator<byte, BasicTypeComparator<byte>>
    {
        public static readonly BytePrimitiveArrayComparator Instance = new BytePrimitiveArrayComparator();

        public BytePrimitiveArrayComparator(bool @ascending = true)
            : base(@ascending, new ByteComparator(ascending))
        { }

        public override int Hash(byte[] record) => record.Sum(field => (int)field);

        public override int Compare(byte[] first, byte[] second)
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

        public override TypeComparator<byte[]> Duplicate()
        {
            var dupe = new BytePrimitiveArrayComparator(Ascending);
            dupe.SetReference(Reference);
            return dupe;
        }
    }
}
