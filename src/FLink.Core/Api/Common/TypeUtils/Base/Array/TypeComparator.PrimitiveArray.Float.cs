using System;
using System.Linq;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class FloatPrimitiveArrayComparator : PrimitiveArrayComparator<float, BasicTypeComparator<float>>
    {
        public static readonly FloatPrimitiveArrayComparator Instance = new FloatPrimitiveArrayComparator();

        public FloatPrimitiveArrayComparator(bool @ascending = true)
            : base(@ascending, new FloatComparator(ascending))
        { }

        public override int Hash(float[] record) => record.Sum(field => (int)field);

        public override int Compare(float[] first, float[] second)
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

        public override TypeComparator<float[]> Duplicate()
        {
            var dupe = new FloatPrimitiveArrayComparator(Ascending);
            dupe.SetReference(Reference);
            return dupe;
        }
    }
}
