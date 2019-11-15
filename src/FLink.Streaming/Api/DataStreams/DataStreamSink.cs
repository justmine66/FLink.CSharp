using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Transformations;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// A Stream Sink. This is used for emitting elements from a streaming topology.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the Stream</typeparam>
    public class DataStreamSink<T>
    {
        public DataStreamSink(DataStream<T> inputStream, StreamSink<T> @operator)
        {
            Transformation = new SinkTransformation<T>(inputStream.Transformation, "Unnamed", @operator, inputStream.ExecutionEnvironment.Parallelism);
        }

        public SinkTransformation<T> Transformation { get; }

        /// <summary>
        /// Sets the parallelism for this sink. The degree must be higher than zero.
        /// </summary>
        /// <param name="parallelism">The parallelism for this sink.</param>
        /// <returns>The sink with set parallelism.</returns>
        public DataStreamSink<T> SetParallelism(int parallelism)
        {
            Transformation.SetParallelism(parallelism);
            return this;
        }
    }
}