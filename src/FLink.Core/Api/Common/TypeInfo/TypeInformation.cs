using System;

namespace FLink.Core.Api.Common.TypeInfo
{
    public abstract class TypeInformation<T> : Type
    {
        /// <summary>
        /// Checks if this type information represents a basic type.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsBasicType { get; }

        public abstract bool IsTupleType { get; }

        /// <summary>
        /// Gets the class of the type represented by this type information.
        /// </summary>
        public abstract Type TypeClass { get; }
    }
}
