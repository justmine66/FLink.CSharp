using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Environment;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// The iterative data stream represents the start of an iteration in a <see cref="DataStream{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the elements in this Stream</typeparam>
    public class IterativeStream<T> : SingleOutputStreamOperator<T>
    {
        protected IterativeStream(StreamExecutionEnvironment environment, Transformation<T> transformation) : base(environment, transformation)
        {
        }
    }
}
