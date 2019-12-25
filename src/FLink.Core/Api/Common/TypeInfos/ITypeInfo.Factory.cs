using System;
using System.Collections.Generic;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// Base class for implementing a type information factory.
    /// A type information factory allows for plugging-in user-defined <see cref="TypeInformation{TType}"/> into the Flink type system.
    /// The factory is called during the type extraction phase if the corresponding type has been annotated with <see cref="TypeInfo"/>. 
    /// </summary>
    /// <typeparam name="T">type for which <see cref="TypeInformation{TType}"/> is created</typeparam>
    public abstract class TypeInfoFactory<T>
    {
        /// <summary>
        /// Creates type information for the type the factory is targeted for.
        /// The parameters provide additional information about the type itself as well as the type's generic type parameters.
        /// </summary>
        /// <param name="t">the exact type the type information is created for; </param>
        /// <param name="genericParameters">mapping of the type's generic type parameters to type information extracted with Flink's type extraction facilities; null values indicate that type information could not be extracted for this parameter </param>
        /// <returns>type information for the type the factory is targeted for</returns>
        public abstract TypeInformation<T> CreateTypeInfo(Type t, IDictionary<string, TypeInformation<object>> genericParameters);
    }
}
