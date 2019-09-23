using FLink.Core.Api.Dag;
using FLink.Core.Util;
using FLink.Streaming.Api.Environment;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// A DataStream represents a stream of elements of the same type.
    /// A DataStream can be transformed into another DataStream by applying a transformation as for example:
    /// </summary>
    /// <typeparam name="T">The type of the elements in this stream.</typeparam>
    public class DataStream<T>
    {
        protected readonly StreamExecutionEnvironment Environment;

        protected readonly Transformation<T> Transformation;

        public DataStream(StreamExecutionEnvironment environment, Transformation<T> transformation)
        {
            Environment = Preconditions.CheckNotNull(environment, "Execution Environment must not be null.");
            Transformation = Preconditions.CheckNotNull(transformation, "Stream Transformation must not be null.");
        }

        /// <summary>
        /// Returns the ID of the <see cref="DataStream{T}"/> in the current <see cref="StreamExecutionEnvironment"/>.
        /// </summary>
        public int Id => Transformation.Id;
    }
}
