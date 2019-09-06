using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Util;

namespace FLink.Core.Api.Dag
{
    /// <summary>
    /// A <see cref="Transformation{T}"/> represents the operation that creates a DataStream.Every DataStream has an underlying <see cref="Transformation{T}"/> that is the origin of said DataStream.
    /// </summary>
    /// <typeparam name="T">The type of the elements that result from this <see cref="Transformation{T}"/></typeparam>
    public abstract class Transformation<T>
    {
        // This is used to assign a unique ID to every Transformation
        protected static int IdCounter = 0;

        public static int GetNewNodeId()
        {
            IdCounter++;
            return IdCounter;
        }

        protected readonly int Id;

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
    }
}
