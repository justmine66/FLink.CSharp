using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// An atomic type is a type that is treated as one indivisible unit and where the entire type acts as a key. The atomic type defines the method to create a comparator for this type as a key. Example atomic types are the basic types (int, long, String, ...) and comparable custom classes.
    /// In contrast to atomic types are composite types, where the type information is aware of the individual fields and individual fields may be used as a key.
    /// </summary>
    public interface IAtomicType<T>
    {
        /// <summary>
        /// Creates a comparator for this type.
        /// </summary>
        /// <param name="sortOrderAscending">True, if the comparator should define the order to be ascending; false, if the comparator should define the order to be descending.</param>
        /// <param name="executionConfig">The config from which the comparator will be parametrized.</param>
        /// <returns>A comparator for this type.</returns>
        TypeComparator<T> CreateComparator(bool sortOrderAscending, ExecutionConfig executionConfig);
    }
}
