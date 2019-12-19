using FLink.Core.Api.Dag;
using FLink.Core.Util;
using FLink.Streaming.Api.Environments;

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

        public SingleOutputStreamOperator(StreamExecutionEnvironment environment, Transformation<TElement> transformation)
            : base(environment, transformation)
        {
        }

        public SingleOutputStreamOperator<TElement> SetParallelism(int parallelism)
        {
            Preconditions.CheckArgument(CanBeParallel || parallelism == 1,
                "The parallelism of non parallel operator must be 1.");

            Transformation.Parallelism = parallelism;

            return this;
        }

        private bool CanBeParallel => !NonParallel;
    }
}
