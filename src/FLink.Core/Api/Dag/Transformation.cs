using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.Operators;
using FLink.Core.Api.Common.Operators.Util;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Core.Util;

namespace FLink.Core.Api.Dag
{
    /// <summary>
    /// A <see cref="Transformation{TElement}"/> represents the operation that creates a DataStream.
    /// Every DataStream has an underlying <see cref="Transformation{T}"/> that is the origin of said DataStream.
    /// API operations such as DataStream.map create a tree of <see cref="Transformation{TElement}"/>s underneath. When the stream program is to be executed, this graph is translated to a StreamGraph using StreamGraphGenerator.
    /// A <see cref="Transformation{TElement}"/> does not necessarily correspond to a physical operation at runtime. Some operations are only logical concepts. Examples of this are union, split/select data stream, partitioning.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements that result from this <see cref="Transformation{TElement}"/></typeparam>
    public abstract class Transformation<TElement> : IEquatable<Transformation<TElement>>
    {
        public const int UpperBoundMaxParallelism = 1 << 15;
        public const int DefaultManagedMemoryWeight = 1;

        protected Transformation(string name, TypeInformation<TElement> outputType, int parallelism)
        {
            Id = NewNodeId;
            Name = Preconditions.CheckNotNull(name);
            OutputType = outputType;
            Parallelism = parallelism;
            SlotSharingGroup = null;
        }

        /// <summary>
        /// This is used to assign a unique ID to every Transformation.
        /// </summary>
        public static int IdCounter { get; private set; }

        public static int NewNodeId => ++IdCounter;

        public int Id { get; set; }

        /// <summary>
        /// Gets and sets the name of <see cref="Transformation{TElement}"/>.
        /// </summary>
        public string Name { get; set; }

        public TypeInformation<TElement> OutputType
        {
            get
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
            set
            {
                if (TypeUsed)
                    throw new IllegalStateException(
                        "TypeInformation cannot be filled in for the type after it has been used. "
                        + "Please make sure that the type info hints are the first call after"
                        + " the transformation function, "
                        + "before any access to types or semantic properties, etc.");

                OutputType = value;
            }
        }

        /// <summary>
        /// This is used to handle MissingTypeInfo. As long as the outputType has not been queried it can still be changed using setOutputType(). Afterwards an exception is thrown when trying to change the output type.
        /// </summary>
        protected bool TypeUsed;

        private int _parallelism;
        /// <summary>
        /// Gets and sets the parallelism of this <see cref="Transformation{TElement}"/>.
        /// </summary>
        public int Parallelism
        {
            get => _parallelism;
            set
            {
                OperatorValidationUtils.ValidateParallelism(_parallelism);
                _parallelism = value;
            }
        }

        private int _maxParallelism;
        /// <summary>
        /// Gets and sets the parallelism of this <see cref="Transformation{TElement}"/>.
        /// </summary>
        public int MaxParallelism
        {
            get => _maxParallelism;
            set
            {
                OperatorValidationUtils.ValidateMaxParallelism(_parallelism);
                _maxParallelism = value;
            }
        }

        /// <summary>
        /// Gets the minimum resources for this stream transformation. It defines the lower limit for dynamic resources resize in future plan.
        /// </summary>
        public ResourceSpec MinResources = ResourceSpec.Default;

        /// <summary>
        /// Gets the preferred resources for this stream transformation. It defines the upper limit for dynamic resource resize in future plan.
        /// </summary>
        public ResourceSpec PreferredResources = ResourceSpec.Default;

        /// <summary>
        /// Gets and sets the managed memory weight which indicates how much this transformation relies on managed memory, so that a transformation highly relies on managed memory would be able to acquire more managed memory in runtime(linear association). The default weight value is 1. Note that currently it's only allowed to set the weight in cases of UNKNOWN resources.
        /// </summary>
        public int ManagedMemoryWeight { get; set; } = DefaultManagedMemoryWeight;

        /// <summary>
        /// Sets the minimum and preferred resources for this stream transformation.
        /// </summary>
        /// <param name="minResources">The minimum resource of this transformation.</param>
        /// <param name="preferredResources">The preferred resource of this transformation.</param>
        public void SetResources(ResourceSpec minResources, ResourceSpec preferredResources)
        {
            MinResources = Preconditions.CheckNotNull(minResources);
            PreferredResources = Preconditions.CheckNotNull(preferredResources);
        }

        /// <summary>
        /// Gets and sets user-specified ID for this transformation. This is used to assign the same operator ID across job restarts. There is also the automatically generated <see cref="Id"/>, which is assigned from a static counter. That field is independent from this.
        /// </summary>
        public string UId { get; set; }

        /// <summary>
        /// Gets the user provided hash.
        /// </summary>
        public string UserProvidedNodeHash { get; private set; }

        /// <summary>
        /// Gets the buffer timeout of this <see cref="Transformation{TElement}"/>.
        /// </summary>
        public long BufferTimeout { get; private set; } = -1;
        /// <summary>
        /// Set the buffer timeout of this <see cref="Transformation{TElement}"/>. The timeout defines how long data may linger in a partially full buffer before being sent over the network.
        /// Lower timeouts lead to lower tail latencies, but may affect throughput. For Flink 1.5+, timeouts of 1ms are feasible for jobs with high parallelism.
        /// A value of -1 means that the default buffer timeout should be used. A value of zero indicates that no buffering should happen, and all records/events should be immediately sent through the network, without additional buffering.
        /// </summary>
        /// <param name="bufferTimeout"></param>
        public void SetBufferTimeout(long bufferTimeout)
        {
            Preconditions.CheckArgument(bufferTimeout >= -1);
            BufferTimeout = bufferTimeout;
        }

        /// <summary>
        /// Gets and sets the slot sharing group name of this transformation.
        /// Parallel instances of operations that are in the same slot sharing group will be co-located in the same TaskManager slot, if possible.
        /// </summary>
        public string SlotSharingGroup { get; set; }

        /// <summary>
        /// This is an internal undocumented feature for now. It is not clear whether this will be supported and stable in the long term.
        /// Gets and sets the key that identifies the co-location group. Operators with the same co-location key will have their corresponding subtasks placed into the same slot by the scheduler.
        /// Setting this to null means there is no co-location constraint.
        /// </summary>
        public string CoLocationGroupKey;

        /// <summary>
        /// Sets an user provided hash for this operator. This will be used AS IS the create the JobVertexID.
        /// The user provided hash is an alternative to the generated hashes, that is considered when identifying an operator through the default hash mechanics fails (e.g. because of changes between Flink versions).
        /// Important: this should be used as a workaround or for trouble shooting. The provided hash needs to be unique per transformation and job. Otherwise, job submission will fail. Furthermore, you cannot assign user-specified hash to intermediate nodes in an operator chain and trying so will let your job fail.
        /// A use case for this is in migration between Flink versions or changing the jobs in a way that changes the automatically generated hashes. In this case, providing the previous hashes directly through this method (e.g. obtained from old logs) can help to reestablish a lost mapping from states to their target operator.
        /// </summary>
        /// <param name="uidHash">The user provided hash for this operator. This will become the JobVertexID, which is shown in the logs and web ui.</param>
        public void SetUidHash(string uidHash)
        {
            Preconditions.CheckNotNull(uidHash);
            Preconditions.CheckArgument(Regex.IsMatch(uidHash, "^[0-9A-Fa-f]{32}$"),
                "Node hash must be a 32 character String that describes a hex code. Found: " + uidHash);

            UserProvidedNodeHash = uidHash;
        }

        /// <summary>
        /// Returns all transitive predecessor <see cref="Transformation{TElement}"/>s of this Transformation. This is, for example, used when determining whether a feedback edge of an iteration actually has the iteration head as a predecessor.
        /// </summary>
        public abstract IList<Transformation<dynamic>> TransitivePredecessors { get; }

        public override string ToString() => GetType().Name + "{" +
                                            "id=" + Id +
                                            ", name='" + Name + '\'' +
                                            ", outputType=" + OutputType +
                                            ", parallelism=" + Parallelism +
                                            '}';

        public bool Equals(Transformation<TElement> other)
        {
            if (other == null) return false;

            if (BufferTimeout != other.BufferTimeout) return false;

            if (Id != other.Id) return false;

            if (Parallelism != other.Parallelism) return false;

            if (!Name.Equals(other.Name)) return false;

            return OutputType?.Equals(other.OutputType) ?? other.OutputType == null;
        }

        public override bool Equals(object obj) => obj is Transformation<TElement> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var result = Id;
                result = 31 * result + Name.GetHashCode();
                result = 31 * result + (OutputType != null ? OutputType.GetHashCode() : 0);
                result = 31 * result + Parallelism;
                result = 31 * result + (int)(BufferTimeout ^ (BufferTimeout >> 32));
                return result;
            }
        }
    }
}
