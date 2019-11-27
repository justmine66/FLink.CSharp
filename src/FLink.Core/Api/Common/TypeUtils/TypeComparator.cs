using FLink.Core.Memory;

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

        /// <summary>
        /// Sets the given element as the comparison reference
        /// </summary>
        /// <param name="toCompare">The element to set as the comparison reference.</param>
        public abstract void SetReference(T toCompare);

        /// <summary>
        /// Checks, whether the given element is equal to the element that has been set as the comparison reference in this comparator instance.
        /// </summary>
        /// <param name="candidate">The candidate to check.</param>
        /// <returns>True, if the element is equal to the comparison reference, false otherwise.</returns>
        public abstract bool EqualToReference(T candidate);

        /// <summary>
        /// The rational behind this method is that elements are typically compared using certain features that are extracted from them, (such de-serializing as a subset of fields). When setting the reference, this extraction happens.The extraction needs happen only once per element, even though an element is typically compared to many other elements when establishing a sorted order.The actual comparison performed by this method may be very cheap, as it happens on the extracted features.
        /// </summary>
        /// <param name="referencedComparator">The type accessors where the element for comparison has been set as reference.</param>
        /// <returns></returns>
        public abstract int CompareToReference(TypeComparator<T> referencedComparator);

        /// <summary>
        /// Compares two records in serialized form. 
        /// </summary>
        /// <param name="firstSource">The input view containing the first record.</param>
        /// <param name="secondSource">The input view containing the second record.</param>
        /// <returns></returns>
        public abstract int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource);

        /// <summary>
        /// True, if the data type supports the creation of a normalized key for comparison, false otherwise.
        /// Checks whether the data type supports the creation of a normalized key for comparison.
        /// </summary>
        public abstract bool SupportsNormalizedKey { get; }

        /// <summary>
        /// True, if the comparator supports that specific form of serialization, false if not.
        /// Check whether this comparator supports to serialize the record in a format that replaces its keys by a normalized key.
        /// </summary>
        public abstract bool SupportsSerializationWithKeyNormalization { get; }

        /// <summary>
        /// Gets the number of bytes that the normalized key would maximally take.
        /// The number of bytes that the normalized key would maximally take.
        /// </summary>
        public abstract int NormalizeKeyLength { get; }

        /// <summary>
        /// Checks, whether the given number of bytes for a normalized is only a prefix to determine the order of elements of the data type for which this comparator provides the comparison methods.For example, if the data type is ordered with respect to an integer value it contains, then this method would return true, if the number of key bytes is smaller than four.
        /// </summary>
        /// <param name="keyBytes"></param>
        /// <returns>True, if the given number of bytes is only a prefix, false otherwise.</returns>
        public abstract bool IsNormalizedKeyPrefixOnly(int keyBytes);

        /// <summary>
        /// Writes a normalized key for the given record into the target byte array, starting at the specified position and writing exactly the given number of bytes.
        /// </summary>
        /// <param name="record">The record for which to create the normalized key.</param>
        /// <param name="target">The byte array into which to write the normalized key bytes.</param>
        /// <param name="offset">The offset in the byte array, where to start writing the normalized key bytes.</param>
        /// <param name="numBytes">The number of bytes to be written exactly. </param>
        public abstract void PutNormalizedKey(T record, MemorySegment target, int offset, int numBytes);

        /// <summary>
        /// Writes the record in such a fashion that all keys are normalizing and at the beginning of the serialized data.
        /// This must only be used when for all the key fields the full normalized key is used.
        /// </summary>
        /// <param name="record">The record object into which to read the record data.</param>
        /// <param name="target">The stream to which to write the data,</param>
        public abstract void WriteWithKeyNormalization(T record, IDataOutputView target);

        /// <summary>
        /// Reads the record back while de-normalizing the key fields.  
        /// </summary>
        /// <param name="reuse">The reuse object into which to read the record data.</param>
        /// <param name="source">The stream from which to read the data,</param>
        /// <returns></returns>
        public abstract T ReadWithKeyDeNormalization(T reuse, IDataInputView source);

        /// <summary>
        /// True, if all normalized key comparisons should invert the sign of the comparison result,True, if all normalized key comparisons should invert the sign of the comparison result, false if the normalized key should be used as is.
        /// Flag whether normalized key comparisons should be inverted key should be interpreted inverted, i.e. descending.
        /// </summary>
        public abstract bool InvertNormalizedKey { get; }

        /// <summary>
        /// Creates a copy of this class. The copy must be deep such that no state set in the copy affects this instance of the comparator class.
        /// </summary>
        /// <returns>A deep copy of this comparator instance.</returns>
        public abstract TypeComparator<T> Duplicate();

        /// <summary>
        /// Extracts the key fields from a record. This is for use by the PairComparator to provide interoperability between different record types. Note, that at least one key should be extracted.
        /// </summary>
        /// <param name="record">The record that contains the key(s)</param>
        /// <param name="target">The array to write the key(s) into.</param>
        /// <param name="index">The offset of the target array to start writing into.</param>
        /// <returns>the number of keys added to target.</returns>
        public abstract int ExtractKeys(object record, object[] target, int index);

        public abstract TypeComparator<object>[] FlatComparators { get; }
    }
}
