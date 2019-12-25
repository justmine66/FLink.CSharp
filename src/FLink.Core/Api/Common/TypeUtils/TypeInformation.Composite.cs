using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Exceptions;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeUtils
{
    /// <summary>
    /// Base type information class for Tuple and Pojo types.
    /// The class is taking care of serialization and comparators for Tuples as well.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CompositeType<T> : TypeInformation<T>, IEquatable<CompositeType<T>>
    {
        private readonly Type _typeClass; 

        protected CompositeType(Type typeClass) => _typeClass = Preconditions.CheckNotNull(typeClass);

        public override Type TypeClass => _typeClass;

        /// <summary>
        /// Gets the flat field descriptors for the given field expression.
        /// </summary>
        /// <param name="fieldExpression">The field expression for which the flat field descriptors are computed.</param>
        /// <returns>The list of descriptors for the flat fields which are specified by the field expression.</returns>
        public List<FlatFieldDescriptor> GetFlatFields(string fieldExpression)
        {
            var result = new List<FlatFieldDescriptor>();
            GetFlatFields(fieldExpression, 0, result);
            return result;
        }

        /// <summary>
        /// Computes the flat field descriptors for the given field expression with the given offset.
        /// </summary>
        /// <param name="fieldExpression">The field expression for which the FlatFieldDescriptors are computed.</param>
        /// <param name="offset">The offset to use when computing the positions of the flat fields.</param>
        /// <param name="result">The list into which all flat field descriptors are inserted.</param>
        public abstract void GetFlatFields(string fieldExpression, int offset, IList<FlatFieldDescriptor> result);

        /// <summary>
        /// Returns the type of the (nested) field at the given field expression position.
        /// Wildcards are not allowed.
        /// </summary>
        /// <typeparam name="TX"></typeparam>
        /// <param name="fieldExpression"></param>
        /// <returns></returns>
        public abstract TypeInformation<TX> GetTypeAt<TX>(string fieldExpression);

        /// <summary>
        /// Returns the type of the (unnested) field at the given field position.
        /// </summary>
        /// <typeparam name="TX">The position of the (unnested) field in this composite type.</typeparam>
        /// <param name="pos">The type of the field at the given position.</param>
        /// <returns></returns>
        public abstract TypeInformation<TX> GetTypeAt<TX>(int pos);

        protected abstract ITypeComparatorBuilder<T> CreateTypeComparatorBuilder();

        /// <summary>
        /// Generic implementation of the comparator creation. Composite types are supplying the infrastructure to create the actual comparators.
        /// </summary>
        /// <param name="logicalKeyFields"></param>
        /// <param name="orders"></param>
        /// <param name="logicalFieldOffset"></param>
        /// <param name="config"></param>
        /// <returns>The comparator</returns>
        public TypeComparator<T> CreateComparator(
            int[] logicalKeyFields,
            bool[] orders,
            int logicalFieldOffset,
            ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        public override bool IsKeyType
        {
            get
            {
                for (var i = 0; i < Arity; i++)
                    if (!GetTypeAt<object>(i).IsKeyType)
                        return false;

                return true;
            }
        }

        public override bool IsSortKeyType
        {
            get
            {
                for (var i = 0; i < Arity; i++)
                    if (!GetTypeAt<object>(i).IsSortKeyType)
                        return false;

                return true;
            }
        }

        /// <summary>
        /// Gets the names of the composite fields of this type. The order of the returned array must be consistent with the internal field index ordering.
        /// </summary>
        public abstract string[] FieldNames { get; }

        /// <summary>
        /// True if this type has an inherent ordering of the fields, such that a user can always be sure in which order the fields will be in. This is true for Tuples and Case Classes. It is not true for Regular Java Objects, since there, the ordering of the fields can be arbitrary. This is used when translating a DataSet or DataStream to an Expression Table, when initially renaming the fields of the underlying type.
        /// </summary>
        public virtual bool HasDeterministicFieldOrder => false;

        public bool HasField(string fieldName) => GetFieldIndex(fieldName) >= 0;

        /// <summary>
        /// Gets the field index of the composite field of the given name.
        /// </summary>
        /// <param name="fieldName">The field index or -1 if this type does not have a field of the given name.</param>
        /// <returns></returns>
        public abstract int GetFieldIndex(string fieldName);

        public class InvalidFieldReferenceException : IllegalArgumentException
        {
            public InvalidFieldReferenceException(string message)
                : base(message)
            {
            }
        }

        public bool Equals(CompositeType<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return _typeClass == other._typeClass;
        }

        public override bool Equals(object obj) => obj is CompositeType<T> other && Equals(other);

        public override int GetHashCode() => _typeClass.GetHashCode();

        public override string ToString() => GetType().Name + "<" + _typeClass.Name + ">";

        public interface ITypeComparatorBuilder<T1>
        {
            void InitializeTypeComparatorBuilder(int size);

            void AddComparatorField<TType>(int fieldId, TypeComparator<TType> comparator);

            TypeComparator<T1> CreateTypeComparator(ExecutionConfig config);
        }

        public class FlatFieldDescriptor
        {
            public FlatFieldDescriptor(int keyPosition, TypeInformation<object> type)
            {
                if (type is CompositeType<object>)
                {
                    throw new IllegalArgumentException("A flattened field can not be a composite type");
                }

                KeyPosition = keyPosition;
                Type = type;
            }

            public int KeyPosition { get; }
            public TypeInformation<object> Type { get; }

            public override string ToString() => $"FlatFieldDescriptor [position={KeyPosition} typeInfo={Type}]";
        }
    }
}
