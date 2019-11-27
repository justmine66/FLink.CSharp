using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public abstract class BasicTypeComparator<T> : TypeComparator<T> where T : IComparable<T>
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
            var comp = ((BasicTypeComparator<T>)referencedComparator)._reference.CompareTo(_reference);
            return AscendingComparison ? comp : -comp;
        }

        public override int Compare(T first, T second)
        {
            var cmp = first.CompareTo(second);
            return AscendingComparison ? cmp : -cmp;
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
