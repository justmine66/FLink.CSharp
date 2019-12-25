using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public abstract class BasicTypeComparator<T> : TypeComparator<T>
    {
        private T _reference;

        protected readonly bool AscendingComparison;

        protected BasicTypeComparator(bool ascending)
        {
            AscendingComparison = ascending;
        }

        public override int Hash(T record) => record.GetHashCode();

        public override void SetReference(T toCompare) => _reference = toCompare;

        public override bool EqualToReference(T candidate) => candidate.Equals(_reference);

        public override int CompareToReference(TypeComparator<T> referencedComparator)
        {
            if (referencedComparator is BasicTypeComparator<T> comparator &&
                comparator._reference is IComparable<T> comparable)
            {
                var comp = comparable.CompareTo(_reference);

                return AscendingComparison ? comp : -comp;
            }

            throw new InvalidCastException(nameof(referencedComparator));
        }

        public override int Compare(T first, T second)
        {
            if (first is IComparable<T> comparable)
            {
                var cmp = comparable.CompareTo(second);
                return AscendingComparison ? cmp : -cmp;
            }

            throw new InvalidCastException(nameof(first));
        }

        public override bool InvertNormalizedKey => !AscendingComparison;

        public override bool SupportsSerializationWithKeyNormalization => false;

        public override void WriteWithKeyNormalization(T record, IDataOutputView target) => throw new InvalidOperationException();

        public override int ExtractKeys(object record, object[] target, int index)
        {
            target[index] = record;
            return 1;
        }

        public override TypeComparator<object>[] FlatComparators => new[] { this as TypeComparator<object> };

        public override T ReadWithKeyDeNormalization(T reuse, IDataInputView source) => throw new InvalidOperationException();
    }
}
