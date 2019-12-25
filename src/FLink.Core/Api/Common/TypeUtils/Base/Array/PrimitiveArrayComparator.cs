using System;
using FLink.Core.Exceptions;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base.Array
{
    public abstract class PrimitiveArrayComparator<TElement, TComparator> : TypeComparator<TElement[]>
        where TComparator : BasicTypeComparator<TElement>
    {
        public bool Ascending { get; }
        public TElement[] Reference { get; set; }
        public TComparator Comparator { get; }

        protected PrimitiveArrayComparator(bool ascending, TComparator comparator)
        {
            Ascending = ascending;
            Comparator = comparator;
        }

        public override void SetReference(TElement[] toCompare) => Reference = toCompare;

        public override bool EqualToReference(TElement[] candidate) => Compare(Reference, candidate) == 0;

        public override int CompareToReference(TypeComparator<TElement[]> referencedComparator)
        {
            if (referencedComparator is PrimitiveArrayComparator<TElement, TComparator> comparator)
            {
                return Compare(comparator.Reference, Reference);
            }

            throw new InvalidCastException(nameof(referencedComparator));
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            var firstCount = firstSource.ReadInt();
            var secondCount = secondSource.ReadInt();

            for (var x = 0; x < Math.Min(firstCount, secondCount); x++)
            {
                var result = Comparator.CompareSerialized(firstSource, secondSource);
                if (result != 0)
                {
                    return result;
                }
            }

            var cmp = firstCount - secondCount;

            return Ascending ? cmp : -cmp;
        }

        public override int ExtractKeys(object record, object[] target, int index)
        {
            target[index] = record;
            return 1;
        }

        public override TypeComparator<object>[] FlatComparators => new TypeComparator<object>[1] { this as TypeComparator<object> };

        public override bool SupportsNormalizedKey => false;

        public override bool SupportsSerializationWithKeyNormalization => false;

        public override int NormalizeKeyLength => 0;

        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => throw new UnSupportedOperationException();

        public override void PutNormalizedKey(TElement[] record, MemorySegment target, int offset, int numBytes) => throw new UnSupportedOperationException();

        public override void WriteWithKeyNormalization(TElement[] record, IDataOutputView target) => throw new UnSupportedOperationException();

        public override TElement[] ReadWithKeyDeNormalization(TElement[] reuse, IDataInputView source) => throw new UnSupportedOperationException();

        public override bool InvertNormalizedKey => !Ascending;
    }
}
