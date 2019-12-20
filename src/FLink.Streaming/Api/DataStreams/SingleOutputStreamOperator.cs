using FLink.Core.Api.Common.Operators.Util;
using FLink.Core.Api.Dag;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Transformations;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// <see cref="SingleOutputStreamOperator{T}"/> represents a user defined transformation applied on a <see cref="DataStream{T}"/> with one predefined output type.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements in this Stream</typeparam>
    public class SingleOutputStreamOperator<TElement> : DataStream<TElement>
    {
        /** Indicate this is a non-parallel operator and cannot set a non-1 degree of parallelism. **/
        protected bool NonParallel = false;

        private bool CanBeParallel => !NonParallel;

        public SingleOutputStreamOperator(StreamExecutionEnvironment environment, Transformation<TElement> transformation)
            : base(environment, transformation)
        { }

        /// <summary>
        /// Sets the parallelism for this operator.
        /// </summary>
        /// <param name="parallelism">The parallelism for this operator.</param>
        /// <returns>The operator with set parallelism.</returns>
        public SingleOutputStreamOperator<TElement> SetParallelism(int parallelism)
        {
            Preconditions.CheckArgument(CanBeParallel || parallelism == 1,
                "The parallelism of non parallel operator must be 1.");

            Transformation.Parallelism = parallelism;

            return this;
        }

        /// <summary>
        /// Sets the maximum parallelism of this operator.
        /// The maximum parallelism specifies the upper bound for dynamic scaling.
        /// It also defines the number of key groups used for partitioned state.
        /// </summary>
        /// <param name="maxParallelism">Maximum parallelism</param>
        /// <returns>The operator with set maximum parallelism</returns>
        public SingleOutputStreamOperator<TElement> SetMaxParallelism(int maxParallelism)
        {
            OperatorValidationUtils.ValidateMaxParallelism(maxParallelism, CanBeParallel);

            Transformation.MaxParallelism = maxParallelism;

            return this;
        }

        /// <summary>
        /// Gets the name of the current data stream.
        /// This name is used by the visualization and logging during runtime.
        /// </summary>
        public string Name => Transformation.Name;

        /// <summary>
        /// Sets the name of the current data stream.
        /// This name is used by the visualization and logging during runtime.
        /// </summary>
        /// <param name="name">The named operator.</param>
        /// <returns>The operator with the user provided name.</returns>
        public SingleOutputStreamOperator<TElement> SetName(string name)
        {
            Transformation.Name = name;

            return this;
        }

        /// <summary>
        /// Sets an ID for this operator.
        /// The specified ID is used to assign the same operator ID across job submissions (for example when starting a job from a savepoint).
        /// Important: this ID needs to be unique per transformation and job. Otherwise, job submission will fail.
        /// </summary>
        /// <param name="uid">The unique user-specified ID of this transformation.</param>
        /// <returns>operator with the specified ID.</returns>
        public SingleOutputStreamOperator<TElement> SetUId(string uid)
        {
            Transformation.UId = uid;

            return this;
        }

        /// <summary>
        /// Sets an user provided hash for this operator. This will be used AS IS the create the JobVertexId.
        /// 
        /// The user provided hash is an alternative to the generated hashes, that is considered when identifying an operator through the default hash mechanics fails (e.g. because of changes between Flink versions).
        /// 
        /// Important: this should be used as a workaround or for trouble shooting. The provided hash needs to be unique per transformation and job. Otherwise, job submission  will fail. Furthermore, you cannot assign user-specified hash to intermediate nodes in an operator chain and trying so will let your job fail.
        ///
        /// A use case for this is in migration between Flink versions or changing the jobs in a way that changes the automatically generated hashes. In this case, providing the previous hashes directly through this method (e.g. obtained from old logs) can help to reestablish a lost mapping from states to their target operator.
        /// </summary>
        /// <param name="uidHash">The user provided hash for this operator. This will become the JobVertexID, which is shown in the logs and web ui.</param>
        /// <returns>The operator with the user provided hash.</returns>
        public SingleOutputStreamOperator<TElement> SetUIdHash(string uidHash)
        {
            Transformation.SetUIdHash(uidHash);
            return this;
        }

        /// <summary>
        /// Sets the parallelism and maximum parallelism of this operator to one.
        /// And mark this operator cannot set a non-1 degree of parallelism.
        /// </summary>
        /// <returns>The operator with only one parallelism.</returns>
        public SingleOutputStreamOperator<TElement> ForceNonParallel()
        {
            Transformation.Parallelism = 1;
            Transformation.MaxParallelism = 1;
            NonParallel = true;

            return this;
        }

        /// <summary>
        /// Sets the buffering timeout for data produced by this operation.
        /// The timeout defines how long data may linger in a partially full buffer before being sent over the network.
        ///
        /// Lower timeouts lead to lower tail latencies, but may affect throughput. Timeouts of 1 ms still sustain high throughput, even for jobs with high parallelism.
        /// 
        /// A value of '-1' means that the default buffer timeout should be used. A value of '0' indicates that no buffering should happen, and all records/events should be immediately sent through the network, without additional buffering.
        /// </summary>
        /// <param name="timeoutMillis">The maximum time between two output flushes.</param>
        /// <returns>The operator with buffer timeout set.</returns>
        public SingleOutputStreamOperator<TElement> SetBufferTimeout(long timeoutMillis)
        {
            Preconditions.CheckArgument(timeoutMillis >= -1, "timeout must be >= -1");

            Transformation.BufferTimeout = timeoutMillis;

            return this;
        }

        /// <summary>
        /// Turns off chaining for this operator so thread co-location will not be used as an optimization. 
        /// Chaining can be turned off for the whole job by <see cref="StreamExecutionEnvironment.DisableOperatorChaining()"/> however it is not advised for performance considerations.
        /// </summary>
        /// <returns>The operator with chaining disabled</returns>
        public SingleOutputStreamOperator<TElement> DisableChaining() => SetChainingStrategy(ChainingStrategy.Never);

        /// <summary>
        /// Starts a new task chain beginning at this operator. This operator will not be chained (thread co-located for increased performance) to any previous tasks even if possible.
        /// </summary>
        /// <returns>The operator with chaining set.</returns>
        public SingleOutputStreamOperator<TElement> StartNewChain() => SetChainingStrategy(ChainingStrategy.Head);

        /// <summary>
        /// Sets the <see cref="ChainingStrategy"/> for the given operator affecting the way operators will possibly be co-located on the same thread for increased performance.
        /// </summary>
        /// <param name="strategy">The selected <see cref="ChainingStrategy"/></param>
        /// <returns>The operator with the modified chaining strategy</returns>
        private SingleOutputStreamOperator<TElement> SetChainingStrategy(ChainingStrategy strategy)
        {
            if (Transformation is PhysicalTransformation<object> physical)
            {
                physical.SetChainingStrategy(strategy);
            }
            else
            {
                throw new UnSupportedOperationException($"Cannot set chaining strategy on {Transformation}");
            }

            return this;
        }

        /// <summary>
        /// Sets the slot sharing group of this operation.
        /// Parallel instances of operations that are in the same slot sharing group will be co-located in the same TaskManager slot, if possible.
        /// Operations inherit the slot sharing group of input operations if all input operations are in the same slot sharing group and no slot sharing group was explicitly specified.
        /// Initially an operation is in the default slot sharing group. An operation can be put into the default group explicitly by setting the slot sharing group to {@code "default"}.
        /// </summary>
        /// <param name="slotSharingGroup">The slot sharing group name.</param>
        /// <returns>The operator with the slot sharing group name.</returns>
        public SingleOutputStreamOperator<TElement> SetSlotSharingGroup(string slotSharingGroup)
        {
            Transformation.SlotSharingGroup = slotSharingGroup;

            return this;
        }
    }
}
