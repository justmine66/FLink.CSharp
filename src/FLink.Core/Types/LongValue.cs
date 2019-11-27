using System;
using FLink.Core.Memory;

namespace FLink.Core.Types
{
    /// <summary>
    /// Boxed serializable and comparable long integer type, representing the primitive type long.
    /// </summary>
    public class LongValue : IResettableValue<LongValue>, ICopyableValue<LongValue>, IEquatable<LongValue>, IComparable<LongValue>
    {
        public long Value;

        public LongValue(long value) => Value = value;

        public void SetValue(long value) => Value = value;
        public void SetValue(LongValue value) => Value = value.Value;

        public LongValue Copy()
        {
            throw new System.NotImplementedException();
        }

        public void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public int BinaryLength { get; }

        public void CopyTo(LongValue target)
        {
            throw new System.NotImplementedException();
        }

        public int GetBinaryLength()
        {
            throw new System.NotImplementedException();
        }

        public void Read(IDataInputView input)
        {
            throw new System.NotImplementedException();
        }

        public void Write(IDataOutputView output)
        {
            throw new System.NotImplementedException();
        }

        public override bool Equals(object obj) => obj is LongValue other && Equals(other);

        public bool Equals(LongValue other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override int GetHashCode() => 43 + (int)(Value ^ Value >> 32);

        public int CompareTo(LongValue o)
        {
            var other = o.Value;
            return Value < other ? -1 : Value > other ? 1 : 0;
        }
    }
}
