using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Transformations;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// The DataStreamSource represents the starting point of a DataStream.
    /// </summary>
    /// <typeparam name="T">Type of the elements in the DataStream created from the this source.</typeparam>
    public class DataStreamSource<T> : SingleOutputStreamOperator<T>
    {
        private readonly bool _isParallel;

        public DataStreamSource(
            StreamExecutionEnvironment env,
            TypeInformation<T> outTypeInfo,
            StreamSource<T, ISourceFunction<T>> @operator,
            bool isParallel, string sourceName)
            : base(env, new SourceTransformation<T>(sourceName, @operator, outTypeInfo, env.Parallelism))
        {
            _isParallel = isParallel;
            if (!isParallel) SetParallelism(1);
        }

        public new DataStreamSource<T> SetParallelism(int parallelism)
        {
            if (parallelism != 1 && !_isParallel)
            {
                throw new IllegalArgumentException("Source: " + Transformation.Id + " is not a parallel source");
            }

            base.SetParallelism(parallelism);
            return this;
        }
    }
}
