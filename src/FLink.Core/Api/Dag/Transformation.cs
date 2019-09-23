using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Util;

namespace FLink.Core.Api.Dag
{
    /// <summary>
    /// A <see cref="Transformation{T}"/> represents the operation that creates a DataStream.
    /// Every DataStream has an underlying <see cref="Transformation{T}"/> that is the origin of said DataStream.
    /// </summary>
    /// <typeparam name="T">The type of the elements that result from this <see cref="Transformation{T}"/></typeparam>
    public abstract class Transformation<T>
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

        protected TypeInformation<T> OutputType;

        private int _parallelism;

        private string _slotSharingGroup;

        protected Transformation(string name, TypeInformation<T> outputType, int parallelism)
        {
            Id = GetNewNodeId();
            Name = Preconditions.CheckNotNull(name);
            OutputType = outputType;
            _parallelism = parallelism;
            _slotSharingGroup = null;
        }

        public void SetParallelism(int parallelism)
        {
            Preconditions.CheckArgument(
                parallelism > 0 || parallelism == ExecutionConfig.DefaultParallelism,
                "The parallelism must be at least one, or ExecutionConfig.DefaultParallelism (use system default).");

            _parallelism = parallelism;
        }

        public int GetParallelism() => _parallelism;
    }
}
