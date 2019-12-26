using System;
using System.Linq;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public class DoublePrimitiveArrayComparator : PrimitiveArrayComparator<double, BasicTypeComparator<double>>
    {
        public static readonly DoublePrimitiveArrayComparator Instance = new DoublePrimitiveArrayComparator();

        public DoublePrimitiveArrayComparator(bool @ascending = true)
            : base(@ascending, new DoubleComparator(ascending))
        { }

        public override int Hash(double[] record) => record.Sum(field => (int)field);

        public override int Compare(double[] first, double[] second)
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

        public override TypeComparator<double[]> Duplicate()
        {
            var dupe = new DoublePrimitiveArrayComparator(Ascending);
            dupe.SetReference(Reference);
            return dupe;
        }
    }
}
