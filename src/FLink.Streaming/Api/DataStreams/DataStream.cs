using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Core.Util;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Functions.Sink;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// A DataStream represents a stream of elements of the same type.
    /// A DataStream can be transformed into another DataStream by applying a transformation as for example:
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in this stream.</typeparam>
    public class DataStream<TElement>
    {
        public StreamExecutionEnvironment Environment { get; }

        public Transformation<TElement> Transformation { get; }

        public DataStream(StreamExecutionEnvironment environment, Transformation<TElement> transformation)
        {
            Environment = Preconditions.CheckNotNull(environment, "Execution Environment must not be null.");
            Transformation = Preconditions.CheckNotNull(transformation, "Stream Transformation must not be null.");
        }

        /// <summary>
        /// Returns the ID of the <see cref="DataStream{T}"/> in the current <see cref="StreamExecutionEnvironment"/>.
        /// </summary>
        public int Id => Transformation.Id;

        /// <summary>
        /// Applies a FlatMap transformation on a <see cref="DataStream{T}"/>. The transformation calls a <see cref="IFlatMapFunction{TInput,TOutput}"/> for each element of the DataStream. Each FlatMapFunction call can return any number of elements including none. The user can also extend <see cref="IRichFunction"/> to gain access to other features provided by the  <see cref="IRichFunction"/> interface.
        /// </summary>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="flatMapper">The MapFunction that is called for each element of the DataStream.</param>
        /// <returns>The transformed <see cref="DataStream{T}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> FlatMap<TOutput>(IFlatMapFunction<TElement, TOutput> flatMapper)
        {
            return null;
        }

        /// <summary>
        /// Applies a Map transformation on a <see cref="DataStream{T}"/>. The transformation calls a <see cref="IMapFunction{TInput,TOutput}"/> for each element of the DataStream.
        /// </summary>
        /// <typeparam name="TOutput">The output type</typeparam>
        /// <param name="mapper">The MapFunction that is called for each element of the DataStream.</param>
        /// <returns>The transformed <see cref="DataStream{T}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Map<TOutput>(IMapFunction<TElement, TOutput> mapper)
        {
            return null;
        }

        /// <summary>
        /// Partitions the operator state of a <see cref="DataStream{T}"/> using field expressions. A field expression is either the name of a public field or a getter method with parentheses of the <see cref="DataStream{T}"/>'s underlying type.
        /// </summary>
        /// <param name="fields">One or more field expressions on which the state of the <see cref="DataStream{T}"/> operators will be partitioned.</param>
        /// <returns>The <see cref="DataStream{T}"/> with partitioned state (i.e. KeyedStream)</returns>
        public KeyedStream<TElement, object> KeyBy(params string[] fields)
        {
            return null;
        }

        /// <summary>
        /// Partitions the operator state of a <see cref="DataStream{TElement}"/> by the given key positions.
        /// </summary>
        /// <param name="fields">The position of the fields on which the <see cref="DataStream{TElement}"/> will be grouped.</param>
        /// <returns>The <see cref="DataStream{TElement}"/> with partitioned state (i.e. <see cref="KeyedStream{T,TKey}"/>)</returns>
        public KeyedStream<TElement, object> KeyBy(params int[] fields)
        {
            return null;
        }

        /// <summary>
        /// Writes a DataStream to the standard output stream (stdout).
        /// For each element of the DataStream the result of <see cref="object.ToString()"/> is written.
        /// NOTE: This will print to stdout on the machine where the code is executed, i.e. the FLink worker.
        /// </summary>
        /// <returns>The closed DataStream.</returns>
        public DataStreamSink<TElement> Print()
        {
            return null;
        }

        /// <summary>
        /// Assigns timestamps to the elements in the data stream and periodically creates watermarks to signal event time progress.
        /// This method creates watermarks periodically (for example every second), based on the watermarks indicated by the given watermark generator. Even when no new elements in the stream arrive, the given watermark generator will be periodically checked for new watermarks. The interval in which watermarks are generated is defined in <see cref="FLink.Core.Api.Common.ExecutionConfig.SetAutoWatermarkInterval"/>.
        /// Use this method for the common cases, where some characteristic over all elements should generate the watermarks, or where watermarks are simply trailing behind the wall clock time by a certain amount.
        /// For the second case and when the watermarks are required to lag behind the maximum timestamp seen so far in the elements of the stream by a fixed amount of time, and this amount is known in advance, use the  <see cref="FLink.Streaming.Api.Functions.Timestamps.BoundedOutOfOrdernessTimestampExtractor{T}"/>.
        /// For cases where watermarks should be created in an irregular fashion, for example based on certain markers that some element carry, use the <see cref="IAssignerWithPunctuatedWatermarks{T}"/>.
        /// </summary>
        /// <param name="timestampAndWatermarkAssigner">The implementation of the timestamp assigner and watermark generator.</param>
        /// <returns>The stream after the transformation, with assigned timestamps and watermarks.</returns>
        public SingleOutputStreamOperator<TElement> AssignTimestampsAndWatermarks(
            IAssignerWithPeriodicWatermarks<TElement> timestampAndWatermarkAssigner)
        {
            return null;
        }

        /// <summary>
        /// Assigns timestamps to the elements in the data stream and creates watermarks to signal event time progress based on the elements themselves.
        /// </summary>
        /// <param name="timestampAndWatermarkAssigner">The implementation of the timestamp assigner and watermark generator.</param>
        /// <returns>The stream after the transformation, with assigned timestamps and watermarks.</returns>
        public SingleOutputStreamOperator<TElement> AssignTimestampsAndWatermarks(
            IAssignerWithPunctuatedWatermarks<TElement> timestampAndWatermarkAssigner)
        {
            return null;
        }

        /// <summary>
        /// Applies a Filter transformation on a <see cref="DataStream{T}"/>.
        /// The transformation calls a <see cref="IFilterFunction{T}"/> for each element of the DataStream and retains only those element for which the function returns true. Elements for which the function returns false are filtered. The user can also extend <see cref="IRichFunction"/> to gain access to other features provided by the <see cref="IRichFunction"/> interface.
        /// </summary>
        /// <param name="filter">The FilterFunction that is called for each element of the DataStream.</param>
        /// <returns>The filtered DataStream.</returns>
        public SingleOutputStreamOperator<TElement> Filter(IFilterFunction<TElement> filter)
        {
            return null;
        }

        /// <summary>
        /// Adds the given sink to this DataStream. Only streams with sinks added will be executed once the <see cref="StreamContextEnvironment.Execute()"/> method is called.
        /// </summary>
        /// <param name="sinkFunction">The object containing the sink's invoke function.</param>
        /// <returns>The closed DataStream.</returns>
        public DataStreamSink<TElement> AddSink(ISinkFunction<TElement> sinkFunction)
        {
            return null;
        }

        #region [ private members ] 

        private SingleOutputStreamOperator<TReturn> DoTransform<TReturn>(
    string operatorName,
    TypeInformation<TReturn> outTypeInfo,
    IStreamOperatorFactory<TReturn> operatorFactory)
        {
            return null;
        }

        #endregion
    }
}
