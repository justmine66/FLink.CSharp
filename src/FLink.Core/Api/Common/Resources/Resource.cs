using FLink.Core.Util;
using System;

namespace FLink.Core.Api.Common.Resources
{
    /// <summary>
    /// Base class for resources one can specify.
    /// </summary>
    public abstract class Resource : IEquatable<Resource>
    {
        public string Name;

        public decimal Value;

        protected Resource(string name, decimal value)
        {
            Preconditions.CheckNotNull(value);
            Preconditions.CheckArgument(value - decimal.Zero >= 0, "Resource value must be no less than 0");

            Name = Preconditions.CheckNotNull(name);
            Value = value;
        }

        public Resource Merge(Resource other)
        {
            Preconditions.CheckNotNull(other, "Cannot merge with null resources");
            Preconditions.CheckArgument(GetType() == other.GetType(), "Merge with different resource type");
            Preconditions.CheckArgument(Name == other.Name, "Merge with different resource name");

            return Create(Value + other.Value);
        }

        public Resource Subtract(Resource other)
        {
            Preconditions.CheckNotNull(other, "Cannot subtract null resources");
            Preconditions.CheckArgument(GetType() == other.GetType(), "Minus with different resource type");
            Preconditions.CheckArgument(Name == other.Name, "Minus with different resource name");
            Preconditions.CheckArgument(Value - other.Value >= 0, "Try to subtract a larger resource from this one.");

            return Create(Value - other.Value);
        }

        /// <summary>
        /// Create a new instance of the sub resource.
        /// </summary>
        /// <param name="value">The value of the resource</param>
        /// <returns>A new instance of the sub resource</returns>
        public abstract Resource Create(decimal value);

        public bool Equals(Resource other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Value == other.Value;
        }

        public override bool Equals(object obj) => obj is Resource other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var result = Name.GetHashCode();
                result = 31 * result + Value.GetHashCode();
                return result;
            }
        }

        public override string ToString() => $"Resource({Name}: {Value})";
    }
}
