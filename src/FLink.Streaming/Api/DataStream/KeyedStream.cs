using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Environment;

namespace FLink.Streaming.Api.DataStream
{
    public class KeyedStream<T, TKey> : DataStream<T>
    {
        public KeyedStream(StreamExecutionEnvironment environment, Transformation<T> transformation) : base(environment, transformation)
        {
        }
    }
}
