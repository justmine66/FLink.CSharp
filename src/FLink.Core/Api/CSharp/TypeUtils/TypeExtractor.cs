using System;
using System.Collections.Generic;
using System.Linq;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.IO;
using FLink.Core.Api.Common.TypeInfos;
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
            var ti = new TypeExtractor().PrivateCreateTypeInfo<object, object, T>();

            if (ti == null)
            {
                throw new InvalidTypesException("Could not extract type information.");
            }

            return ti;
        }

        public static TypeInformation<TOutput> CreateTypeInfo<TInput1, TInput2, TOutput>(
            Type baseClass,
            Type clazz,
            int returnParamPos,
            TypeInformation<TInput1> in1Type,
            TypeInformation<TInput2> in2Type)
        {
            var ti = new TypeExtractor().PrivateCreateTypeInfo<TInput1, TInput2, TOutput>(baseClass, clazz, returnParamPos, in1Type, in2Type);
            if (ti == null)
            {
                throw new InvalidTypesException("Could not extract type information.");
            }

            return ti;
        }

        private TypeInformation<T> PrivateCreateTypeInfo<TIn1, TIn2, T>()
        {
            var t = typeof(T);
            var typeHierarchy = new List<Type> { t };

            return null;
        }

        private TypeInformation<TOutput> CreateTypeInfoWithTypeHierarchy<TInput1, TInput2, TOutput>(
            IList<Type> typeHierarchy,
            TypeInformation<TInput1> in1Type,
            TypeInformation<TInput2> in2Type)
        {
            var t = typeof(TOutput);

            // check if type information can be created using a type factory
            var typeFromFactory = CreateTypeInfoFromFactory<TInput1, TInput2, TOutput>(t, typeHierarchy, in1Type, in2Type);
            if (typeFromFactory != null)
            {
                return typeFromFactory;
            }
            // check if type is a subclass of tuple
            else if (TypeExtractionUtils.IsClassType(t))
            {

            }

            return null;
        }

        /// <summary>
        /// Creates type information using a factory if for this type or super types. Returns null otherwise.
        /// </summary>
        private TypeInformation<TOutput> CreateTypeInfoFromFactory<TInput1, TInput2, TOutput>(
            Type t,
            IList<Type> typeHierarchy,
            TypeInformation<TInput1> in1Type,
            TypeInformation<TInput2> in2Type)
        {
            var factoryHierarchy = new List<Type>(typeHierarchy);
            var factory = GetClosestFactory<TOutput>(factoryHierarchy, t);
            if (factory == null)
            {
                return null;
            }

            var factoryDefiningType = factoryHierarchy[factoryHierarchy.Count - 1];

            // infer possible type parameters from input
            IDictionary<string,TypeInformation<object>> genericParams;
            if (factoryDefiningType.IsGenericTypeParameter)
            {
                genericParams = new Dictionary<string, TypeInformation<object>>();
                var args = factoryDefiningType.GetGenericParameterConstraints();
            }

            return null;
        }

        /// <summary>
        /// Traverses the type hierarchy up until a type information factory can be found.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="typeHierarchy"></param>
        /// <param name="t">type for which a factory needs to be found</param>
        /// <returns>closest type information factory or null if there is no factory in the type hierarchy</returns>
        private static TypeInfoFactory<TOutput> GetClosestFactory<TOutput>(IList<Type> typeHierarchy, Type t)
        {
            TypeInfoFactory<TOutput> factory = null;


            return factory;
        }

        private TypeInformation<TOutput>[] CreateSubTypesInfo<TIn1, TIn2, TOutput>(
            Type originalType, 
            Type definingType,
            IList<Type> typeHierarchy, 
            TypeInformation<TIn1> in1Type, 
            TypeInformation<TIn2> in2Type, 
            bool lenient)
        {


            return null;
        }

        private TypeInformation<TOutput> PrivateCreateTypeInfo<TInput1, TInput2, TOutput>(
            Type baseClass,
            Type clazz,
            int returnParamPos,
            TypeInformation<TInput1> in1Type,
            TypeInformation<TInput2> in2Type)
        {
            var typeHierarchy = new List<Type>();
            var returnType = GetParameterType(baseClass, typeHierarchy, clazz, returnParamPos);

            TypeInformation<TOutput> typeInfo;

            // return type is a variable -> try to get the type info from the input directly
            return null;
        }

        private Type GetParameterType(Type baseClass, IList<Type> typeHierarchy, Type clazz, int returnParamPos)
        {
            typeHierarchy?.Add(clazz);

            var interfaceTypes = clazz.GetInterfaces().Where(it => it.IsGenericType);

            // search in interfaces for base class
            foreach (var t in interfaceTypes)
            {
                var parameter = GetParameterTypeFromGenericType(baseClass, typeHierarchy, t, returnParamPos);
                if (parameter != null)
                {
                    return parameter;
                }
            }

            // search in superclass for base class
            var t1 = clazz.BaseType;
            var parameter1 = GetParameterTypeFromGenericType(baseClass, typeHierarchy, t1, returnParamPos);
            if (parameter1 != null)
            {
                return parameter1;
            }

            throw new InvalidTypesException("The types of the interface " + baseClass.Name + " could not be inferred. " +
                                            "Support for synthetic interfaces, lambdas, and generic or raw types is limited at this point");
        }

        private Type GetParameterTypeFromGenericType(Type baseClass, IList<Type> typeHierarchy, in Type t, in int returnParamPos)
        {
            return null;
        }

        #endregion
    }
}
