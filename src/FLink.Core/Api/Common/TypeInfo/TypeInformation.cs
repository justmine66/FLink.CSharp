using System;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// The core class of FLink's type system. FLink requires a type information for all types that are used as input or return type of a user function. This type information class acts as the tool to generate serializers and comparators, and to perform semantic checks such as whether the fields that are uses as join/grouping keys actually exist.
    /// </summary>
    /// <typeparam name="T">The type represented by this type information.</typeparam>
    [Serializable]
    public abstract class TypeInformation<T>  
    {
        /// <summary>
        /// Checks if this type information represents a basic type.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsBasicType { get; }

        public abstract bool IsTupleType { get; }

        public abstract int TotalFields();

        /// <summary>
        /// Gets the class of the type represented by this type information.
        /// </summary>
        public abstract Type TypeClass { get; }

        /// <summary>
        /// Creates a serializer for the type. The serializer may use the ExecutionConfig for parameterization.
        /// </summary>
        /// <param name="config">used to parameterize the serializer.</param>
        /// <returns>A serializer for this type.</returns>
        public abstract TypeSerializer<T> CreateSerializer(ExecutionConfig config);
    }
}
