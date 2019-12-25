using System;
using System.Linq;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Types
{
    /// <summary>
    /// A Row can have arbitrary number of fields and contain a set of fields, which may all be different types. The fields in Row can be null. Due to Row is not strongly typed, Flink's type extraction mechanism can't extract correct field types. So that users should manually tell Flink the type information via creating a <see cref="RowTypeInfo"/>.
    /// </summary>
    public class Row : IEquatable<Row>
    {
        /// <summary>
        /// The array to store actual values.
        /// </summary>
        public object[] Fields { get; }

        /// <summary>
        /// Create a new Row instance.
        /// </summary>
        /// <param name="arity">The number of fields in the Row</param>
        public Row(int arity) => Fields = new object[arity];

        /// <summary>
        /// Get the number of fields in the Row.
        /// </summary>
        public int Arity => Fields.Length;

        /// <summary>
        /// Gets the field at the specified position.
        /// </summary>
        /// <param name="index">The position of the field, 0-based.</param>
        /// <returns>The field at the specified position.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown, if the position is negative, or equal to, or larger than the number of fields.</exception>
        private object this[int index] => Fields[index];

        public void SetField(int pos, object value) => Fields[pos] = value;

        /// <summary>
        /// Creates a new Row and assigns the given values to the Row's fields.
        /// This is more convenient than using the constructor.
        /// </summary>
        public static Row Of(params object[] values)
        {
            var length = values.Length;
            var row = new Row(length);

            for (var i = 0; i < length; i++)
            {
                row.SetField(i, values[i]);
            }

            return row;
        }

        public static Row Copy(Row row)
        {
            var length = row.Fields.Length;
            var newRow = new Row(length);

            Array.Copy(row.Fields, 0, newRow.Fields, 0, length);

            return newRow;
        }

        public bool Equals(Row other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Fields.SequenceEqual(other.Fields);
        }

        public override bool Equals(object obj) => obj is Row other && Equals(other);

        public override int GetHashCode() => HashCodeHelper.GetHashCode(Fields);
    }
}
