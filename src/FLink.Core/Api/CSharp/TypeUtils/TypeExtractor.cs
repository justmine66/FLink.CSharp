using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;

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
                new int[] {1, 0},
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
    }
}
