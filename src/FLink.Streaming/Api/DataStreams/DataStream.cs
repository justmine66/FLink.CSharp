using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Api.Dag;
using FLink.Core.Util;
using FLink.Extensions.CSharp;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Functions.Sink;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Evictors;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

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

        /// <summary>
        /// Create a new <see cref="DataStream{TElement}"/> in the given execution environment with partitioning set to forward by default.
        /// </summary>
        /// <param name="environment">The StreamExecutionEnvironment</param>
        /// <param name="transformation"></param>
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
        /// Gets the parallelism for this operator.
        /// </summary>
        public int Parallelism => Transformation.Parallelism;

        protected TFunction Clean<TFunction>(TFunction f) => Environment.Clean(f);

        public TypeInformation<TElement> GetOutputType() => Transformation.GetOutputType();

        /// <summary>
        /// Applies a FlatMap transformation on a <see cref="DataStream{T}"/>. The transformation calls a <see cref="IFlatMapFunction{TInput,TOutput}"/> for each element of the DataStream. Each FlatMapFunction call can return any number of elements including none. The user can also extend <see cref="IRichFunction"/> to gain access to other features provided by the  <see cref="IRichFunction"/> interface.
        /// </summary>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="flatMapper">The MapFunction that is called for each element of the DataStream.</param>
        /// <returns>The transformed <see cref="DataStream{T}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> FlatMap<TOutput>(IFlatMapFunction<TElement, TOutput> flatMapper)
        {
            var outType = TypeExtractor.GetFlatMapReturnTypes(Clean(flatMapper), GetOutputType(), Utils.GetCallLocationName(), true);

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

        #region [ Global Window Utilities ]

        /// <summary>
        /// Windows this <see cref="DataStream{TElement}"/> into sliding time windows.
        /// Note: This operation is inherently non-parallel since all elements have to pass through the same operator instance.
        /// </summary>
        /// <param name="size">The size of the window.</param>
        /// <returns></returns>
        public AllWindowedStream<TElement, TimeWindow> TimeWindowAll(TimeSpan size) =>
            Environment.TimeCharacteristic == TimeCharacteristic.ProcessingTime
                ? WindowAll(TumblingProcessingTimeWindowAssigner<TElement>.Of(size))
                : WindowAll(TumblingEventTimeWindowAssigner<TElement>.Of(size));

        /// <summary>
        /// Windows this <see cref="DataStream{TElement}"/> into sliding time windows.
        /// Note: This operation is inherently non-parallel since all elements have to pass through the same operator instance.
        /// </summary>
        /// <param name="size">The size of the window.</param>
        /// <param name="slide">The slide parameter controls how frequently a sliding window is started.</param>
        /// <returns></returns>
        public AllWindowedStream<TElement, TimeWindow> TimeWindowAll(TimeSpan size, TimeSpan slide) =>
            Environment.TimeCharacteristic == TimeCharacteristic.ProcessingTime
                ? WindowAll(SlidingProcessingTimeWindowAssigner<TElement>.Of(size, slide))
                : WindowAll(SlidingEventTimeWindowAssigner<TElement>.Of(size, slide));

        /// <summary>
        /// Windows this <see cref="DataStream{TElement}"/> into tumbling count windows.
        /// Note: This operation is inherently non-parallel since all elements have to pass through the same operator instance.
        /// </summary>
        /// <param name="size">The size of the windows in number of elements.</param>
        /// <returns></returns>
        public AllWindowedStream<TElement, GlobalWindow> CountWindowAll(long size) =>
            WindowAll(GlobalWindowAssigner<TElement>.Create())
                .Trigger(PurgingWindowTrigger.Of(CountWindowTrigger.Of<TElement, GlobalWindow>(size)));

        /// <summary>
        /// Windows this <see cref="DataStream{TElement}"/> into sliding count windows.
        /// Note: This operation is inherently non-parallel since all elements have to pass through the same operator instance.
        /// </summary>
        /// <param name="size">The size of the windows in number of elements.</param>
        /// <param name="slide">The slide interval in number of elements.</param>
        /// <returns></returns>
        public AllWindowedStream<TElement, GlobalWindow> CountWindowAll(long size, long slide) =>
            WindowAll(GlobalWindowAssigner<TElement>.Create())
                .Evictor(CountWindowEvictor.Of<TElement, GlobalWindow>(size))
                .Trigger(CountWindowTrigger.Of<TElement, GlobalWindow>(slide));

        /// <summary>
        /// Windows this data stream to a <see cref="AllWindowedStream{TElement,TWindow}"/>, which evaluates windows over a non key grouped stream. Elements are put into windows by a <see cref="WindowAssigner{TElement,TWindow}"/>. The grouping of elements is done by window.
        /// Note: This operation is inherently non-parallel since all elements have to pass through the same operator instance.
        /// </summary>
        /// <typeparam name="TWindow"></typeparam>
        /// <param name="assigner">The <see cref="WindowAssigner{TElement,TWindow}"/> that assigns elements to windows.</param>
        /// <returns>The trigger windows data stream.</returns>
        public AllWindowedStream<TElement, TWindow> WindowAll<TWindow>(WindowAssigner<TElement, TWindow> assigner)
            where TWindow : Window => new AllWindowedStream<TElement, TWindow>(this, assigner);

        #endregion

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
