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
    /// <typeparam name="TElement">Type of the elements in the DataStream created from the this source.</typeparam>
    public class DataStreamSource<TElement> : SingleOutputStreamOperator<TElement>
    {
        private readonly bool _isParallel;

        public DataStreamSource(
            StreamExecutionEnvironment env,
            TypeInformation<TElement> outTypeInfo,
            StreamSource<TElement, ISourceFunction<TElement>> @operator,
            bool isParallel,
            string sourceName)
            : base(env, new SourceTransformation<TElement>(sourceName, @operator, outTypeInfo, env.Parallelism))
        {
            _isParallel = isParallel;
            if (!isParallel) SetParallelism(1);
        }

        public DataStreamSource(SingleOutputStreamOperator<TElement> @operator)
            : base(@operator.ExecutionEnvironment, @operator.Transformation)
        {
            _isParallel = true;
        }

        public new DataStreamSource<TElement> SetParallelism(int parallelism)
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
