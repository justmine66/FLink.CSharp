using System;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// The core class of FLink's type system. FLink requires a type information for all types that are used as input or return type of a user function. This type information class acts as the tool to generate serializers and comparators, and to perform semantic checks such as whether the fields that are uses as join/grouping keys actually exist.
    /// </summary>
    /// <typeparam name="TType">The type represented by this type information.</typeparam>
    [Serializable]
    public abstract class TypeInformation<TType>
    {
        /// <summary>
        /// True, if this type information describes a basic type, false otherwise.
        /// Checks if this type information represents a basic type.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsBasicType { get; }

        /// <summary>
        /// True, if this type information describes a tuple type, false otherwise.
        /// Checks if this type information represents a Tuple type. Tuple types are subclasses of the <see cref="System.Tuple"/>.
        /// </summary>
        public abstract bool IsTupleType { get; }

        /// <summary>
        /// Gets the arity of this type - the number of fields without nesting.
        /// </summary>
        public abstract int Arity { get; }

        /// <summary>
        /// Gets the number of logical fields in this type. This includes its nested and transitively nested fields, in the case of composite types. In the example above, the OuterType type has three fields in total.
        /// </summary>
        public abstract int TotalFields { get; }

        /// <summary>
        /// Gets the class of the type represented by this type information.
        /// </summary>
        public abstract Type TypeClass { get; }

        /// <summary>
        /// Creates a serializer for the type. The serializer may use the ExecutionConfig for parameterization.
        /// </summary>
        /// <param name="config">used to parameterize the serializer.</param>
        /// <returns>A serializer for this type.</returns>
        public abstract TypeSerializer<TType> CreateSerializer(ExecutionConfig config);

        public abstract override string ToString();
        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();

        public static TypeInformation<T> Of<T>(Type typeClass)
        {
            return null;
        }
    }
}
