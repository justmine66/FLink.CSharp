using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Core.Util;

namespace FLink.Core.Api.Dag
{
    /// <summary>
    /// A <see cref="Transformation{T}"/> represents the operation that creates a DataStream.
    /// Every DataStream has an underlying <see cref="Transformation{T}"/> that is the origin of said DataStream.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements that result from this <see cref="Transformation{TElement}"/></typeparam>
    public abstract class Transformation<TElement>
    {
        // This is used to assign a unique ID to every Transformation
        public static int IdCounter;

        public static int GetNewNodeId()
        {
            IdCounter++;
            return IdCounter;
        }

        public int Id { get; protected set; }

        protected string Name;

        public TypeInformation<TElement> OutputType;

        /// <summary>
        /// This is used to handle MissingTypeInfo. As long as the outputType has not been queried it can still be changed using setOutputType(). Afterwards an exception is thrown when trying to change the output type.
        /// </summary>
        protected bool TypeUsed;

        public int Parallelism { get; private set; }

        private string _slotSharingGroup;

        protected Transformation(string name, TypeInformation<TElement> outputType, int parallelism)
        {
            Id = GetNewNodeId();
            Name = Preconditions.CheckNotNull(name);
            OutputType = outputType;
            Parallelism = parallelism;
            _slotSharingGroup = null;
        }

        public void SetParallelism(int parallelism)
        {
            Preconditions.CheckArgument(
                parallelism > 0 || parallelism == ExecutionConfig.DefaultParallelism,
                "The parallelism must be at least one, or ExecutionConfig.DefaultParallelism (use system default).");

            Parallelism = parallelism;
        }

        public TypeInformation<TElement> GetOutputType()
        {
            if (OutputType is MissingTypeInfo typeInfo)
                throw new InvalidTypesException(
                    "The return type of function '"
                    + typeInfo.FunctionName
                    + "' could not be determined automatically, due to type erasure. "
                    + "You can give type information hints by using the returns(...) "
                    + "method on the result of the transformation call, or by letting "
                    + "your function implement the 'ResultTypeQueryable' "
                    + "interface.", typeInfo.TypeException);

            TypeUsed = true;
            return OutputType;
        }

        public void SetOutputType(TypeInformation<TElement> outputType)
        {
            if (TypeUsed)
                throw new IllegalStateException(
                    "TypeInformation cannot be filled in for the type after it has been used. "
                    + "Please make sure that the type info hints are the first call after"
                    + " the transformation function, "
                    + "before any access to types or semantic properties, etc.");

            OutputType = outputType;
        }
    }
}
