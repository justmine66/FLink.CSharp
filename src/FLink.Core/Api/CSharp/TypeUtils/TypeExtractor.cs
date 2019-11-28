using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.IO;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.IO;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    /// <summary>
    /// A utility for reflection analysis on classes, to determine the return type of implementations of transformation functions.
    /// </summary>
    public class TypeExtractor
    {
        public static TypeInformation<TOut> GetFlatMapReturnTypes<TIn, TOut>(IFlatMapFunction<TIn, TOut> flatMapInterface,
            TypeInformation<TIn> inType) => GetFlatMapReturnTypes(flatMapInterface, inType, null, false);

        public static TypeInformation<TOut> GetFlatMapReturnTypes<TIn, TOut>(
            IFlatMapFunction<TIn, TOut> flatMapInterface,
            TypeInformation<TIn> inType,
            string functionName,
            bool allowMissing)
            => GetUnaryOperatorReturnType<TIn, TOut>(
                flatMapInterface,
                typeof(IFlatMapFunction<TIn, TOut>),
                0,
                1,
                new int[] { 1, 0 },
                inType,
                functionName,
                allowMissing);

        public static TypeInformation<TOut> GetUnaryOperatorReturnType<TIn, TOut>(
            IFunction function,
            Type baseClass,
            int inputTypeArgumentIndex,
            int outputTypeArgumentIndex,
            int[] lambdaOutputTypeArgumentIndices,
            TypeInformation<TIn> inType,
            string functionName,
            bool allowMissing)
        {
            return null;
        }

        public static TypeInformation<TKey> GetKeySelectorTypes<TElement, TKey>(Functions.IKeySelector<TElement, TKey> keySelector, TypeInformation<TElement> type)
        {
            throw new NotImplementedException();
        }

        public static TypeInformation<TInput> GetInputFormatTypes<TInput, TInputSplit>(IInputFormat<TInput, TInputSplit> inputFormatInterface)
            where TInputSplit : IInputSplit
        {
            throw new NotImplementedException();
        }

        #region [ Create type information ]

        public static TypeInformation<T> CreateTypeInfo<T>()
        {
            return TypeInformation<T>.Of<T>();
        }

        public static TypeInformation<TOutput> CreateTypeInfo<TInput1, TInput2, TOutput>(Type baseClass, Type clazz, int returnParamPos, TypeInformation<TInput1> in1Type, TypeInformation<TInput2> in2Type)
        {
            return null;
        }

        #endregion
    }
}
