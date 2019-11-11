namespace FLink.Core.Api.Common.TypeUtils
{
    /// <summary>
    /// This interface describes the methods that are required for a data type to be handled by the pact runtime. Specifically, this interface contains the methods used for hashing, comparing, and creating auxiliary structures.
    /// </summary>
    /// <typeparam name="T">The data type that the comparator works on.</typeparam>
    public abstract class TypeComparator<T>
    {
        /// <summary>
        /// Computes a hash value for the given record.
        /// The hash value should include all fields in the record relevant to the comparison.
        /// </summary>
        /// <param name="record">The record to be hashed.</param>
        /// <returns>A hash value for the record.</returns>
        public abstract int Hash(T record);

        /// <summary>
        /// Compares two records in object form.
        /// The return value indicates the order of the two in the same way. 
        /// </summary>
        /// <param name="first">The first record.</param>
        /// <param name="second">The second record.</param>
        /// <returns>An integer defining the oder among the objects in the same way.</returns>
        public abstract int Compare(T first, T second);
    }
}
