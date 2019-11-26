using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Exceptions;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeUtils
{
    /// <summary>
    /// Base type information class for Tuple and Pojo types.
    /// The class is taking care of serialization and comparators for Tuples as well.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CompositeType<T> : TypeInformation<T>
    {
        /// <summary>
        /// Gets the type class of the composite type
        /// </summary>
        public Type Type { get; }


        protected CompositeType(Type typeClass)
        {
            Type = Preconditions.CheckNotNull(typeClass);
        }

        /// <summary>
        /// Gets the flat field descriptors for the given field expression.
        /// </summary>
        /// <param name="fieldExpression">The field expression for which the flat field descriptors are computed.</param>
        /// <returns>The list of descriptors for the flat fields which are specified by the field expression.</returns>
        public List<CompositeTypeFlatFieldDescriptor<object>> GetFlatFields(String fieldExpression)
        {
            var result = new List<CompositeTypeFlatFieldDescriptor<object>>();
            GetFlatFields(fieldExpression, 0, result);
            return result;
        }

        /// <summary>
        /// Computes the flat field descriptors for the given field expression with the given offset.
        /// </summary>
        /// <param name="fieldExpression">The field expression for which the FlatFieldDescriptors are computed.</param>
        /// <param name="offset">The offset to use when computing the positions of the flat fields.</param>
        /// <param name="result">The list into which all flat field descriptors are inserted.</param>
        public abstract void GetFlatFields(string fieldExpression, int offset, IList<CompositeTypeFlatFieldDescriptor<object>> result);

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

        protected abstract ICompositeTypeComparatorBuilder<T> CreateTypeComparatorBuilder();

        /// <summary>
        /// Generic implementation of the comparator creation. Composite types are supplying the infrastructure to create the actual comparators.
        /// </summary>
        /// <param name="logicalKeyFields"></param>
        /// <param name="orders"></param>
        /// <param name="logicalFieldOffset"></param>
        /// <param name="config"></param>
        /// <returns>The comparator</returns>
        public TypeComparator<T> CreateComparator(int[] logicalKeyFields, bool[] orders, int logicalFieldOffset,
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
    }

    public interface ICompositeTypeComparatorBuilder<T>
    {
        void InitializeTypeComparatorBuilder(int size);

        void AddComparatorField<TType>(int fieldId, TypeComparator<TType> comparator);

        TypeComparator<T> CreateTypeComparator(ExecutionConfig config);
    }

    public class CompositeTypeFlatFieldDescriptor<T>
    {
        public CompositeTypeFlatFieldDescriptor(int keyPosition, TypeInformation<object> type)
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
