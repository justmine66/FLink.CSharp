using FLink.Core.Api.Dag;
using FLink.Core.Util;
using FLink.Streaming.Api.Environment;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// <see cref="SingleOutputStreamOperator{T}"/> represents a user defined transformation applied on a <see cref="DataStream{T}"/> with one predefined output type.
    /// </summary>
    /// <typeparam name="T">Type of the elements in this Stream</typeparam>
    public class SingleOutputStreamOperator<T> : DataStream<T>
    {
        /** Indicate this is a non-parallel operator and cannot set a non-1 degree of parallelism. **/
        protected bool NonParallel = false;

        protected SingleOutputStreamOperator(StreamExecutionEnvironment environment, Transformation<T> transformation)
            : base(environment, transformation)
        {
        }

        public SingleOutputStreamOperator<T> SetParallelism(int parallelism)
        {
            Preconditions.CheckArgument(CanBeParallel || parallelism == 1,
                "The parallelism of non parallel operator must be 1.");

            Transformation.SetParallelism(parallelism);

            return this;
        }

        private bool CanBeParallel => !NonParallel;
    }
}
