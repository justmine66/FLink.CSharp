using System;
using System.Reflection;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Util;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    /// <summary>
    /// Represent a field definition for <see cref="PojoTypeInfo{T}"/> type of objects.
    /// </summary>
    public class PojoField : IEquatable<PojoField>
    {
        public PojoField(FieldInfo field, TypeInformation<object> type)
        {
            Field = Preconditions.CheckNotNull(field);
            Type = Preconditions.CheckNotNull(type);
        }

        public FieldInfo Field { get; }

        public TypeInformation<object> Type { get; }

        public bool Equals(PojoField other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(Field, other.Field) && Equals(Type, other.Type);
        }

        public override bool Equals(object obj) => obj is PojoField other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Field != null ? Field.GetHashCode() : 0) * 397) ^ (Type != null ? Type.GetHashCode() : 0);
            }
        }
    }
}
