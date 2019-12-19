using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.Operators;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Api.Dag;
using FLink.Core.Util;
using FLink.CSharp;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Functions.Sink;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Transformations;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Evictors;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Util.Keys;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// A DataStream represents a stream of elements of the same type.
    /// A DataStream can be transformed into another DataStream by applying a transformation as for example:
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in this stream.</typeparam>
    public class DataStream<TElement>
    {
        /// <summary>
        /// The Execution Environment for <see cref="DataStream{TElement}"/>.
        /// </summary>
        public StreamExecutionEnvironment ExecutionEnvironment { get; }

        /// <summary>
        /// The Stream Transformation.
        /// </summary>
        public Transformation<TElement> Transformation { get; }

        public ExecutionConfig ExecutionConfig => ExecutionEnvironment.ExecutionConfig;

        /// <summary>
        /// Create a new <see cref="DataStream{TElement}"/> in the given execution environment with partitioning set to forward by default.
        /// </summary>
        /// <param name="environment">The StreamExecutionEnvironment</param>
        /// <param name="transformation"></param>
        public DataStream(StreamExecutionEnvironment environment, Transformation<TElement> transformation)
        {
            ExecutionEnvironment = Preconditions.CheckNotNull(environment, "Execution Environment must not be null.");
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

        protected TFunction Clean<TFunction>(TFunction f) => ExecutionEnvironment.Clean(f);

        public TypeInformation<TElement> Type => Transformation.OutputType;

        /// <summary>
        /// Applies a FlatMap transformation on a <see cref="DataStream{T}"/>. The transformation calls a <see cref="IFlatMapFunction{TInput,TOutput}"/> for each element of the DataStream. Each FlatMapFunction call can return any number of elements including none. The user can also extend <see cref="IRichFunction"/> to gain access to other features provided by the  <see cref="IRichFunction"/> interface.
        /// </summary>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="flatMapper">The MapFunction that is called for each element of the DataStream.</param>
        /// <returns>The transformed <see cref="DataStream{T}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> FlatMap<TOutput>(IFlatMapFunction<TElement, TOutput> flatMapper)
        {
            var outType = TypeExtractor.GetFlatMapReturnTypes(Clean(flatMapper), Type, Utils.GetCallLocationName(), true);

            return Transform("Flat Map", outType, new StreamFlatMap<TElement, TOutput>(Clean(flatMapper)));
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
        public KeyedStream<TElement, string> KeyBy(params string[] fields) =>
            KeyBy<string>(new Keys<TElement>.ExpressionKeys<TElement>(fields, Type));

        /// <summary>
        /// Partitions the operator state of a <see cref="DataStream{TElement}"/> by the given key positions.
        /// </summary>
        /// <param name="fields">The position of the fields on which the <see cref="DataStream{TElement}"/> will be grouped.</param>
        /// <returns>The <see cref="DataStream{TElement}"/> with partitioned state (i.e. <see cref="KeyedStream{T,TKey}"/>)</returns>
        public KeyedStream<TElement, int> KeyBy(params int[] fields)
        {
            if (Type is BasicArrayTypeInfo<TElement[], TElement> ||
                Type is PrimitiveArrayTypeInfo<TElement>)
            {
                return KeyBy(KeySelectorUtil.GetSelectorForArray<TElement, int>(fields, Type));
            }

            return KeyBy<int>(new Keys<TElement>.ExpressionKeys<TElement>(fields, Type));
        }

        /// <summary>
        /// It creates a new <see cref="KeyedStream{TElement,TKey}"/> that uses the provided key for partitioning its operator states.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="selector">The KeySelector to be used for extracting the key for partitioning</param>
        /// <returns>The <see cref="DataStream{TElement}"/> with partitioned state (i.e. KeyedStream)</returns>
        public KeyedStream<TElement, TKey> KeyBy<TKey>(IKeySelector<TElement, TKey> selector)
        {
            Preconditions.CheckNotNull(selector);

            return new KeyedStream<TElement, TKey>(this, Clean(selector));
        }

        /// <summary>
        /// It creates a new <see cref="KeyedStream{TElement,TKey}"/> that uses the provided key with explicit type information for partitioning its operator states.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="selector">The KeySelector to be used for extracting the key for partitioning.</param>
        /// <param name="keyType">The type information describing the key type.</param>
        /// <returns>The <see cref="DataStream{TElement}"/> with partitioned state (i.e. KeyedStream)</returns>
        public KeyedStream<TElement, TKey> KeyBy<TKey>(IKeySelector<TElement, TKey> selector, TypeInformation<TKey> keyType)
        {
            Preconditions.CheckNotNull(selector);
            Preconditions.CheckNotNull(keyType);

            return new KeyedStream<TElement, TKey>(this, Clean(selector), keyType);
        }

        public KeyedStream<TElement, TKey> KeyBy<TKey>(Func<TElement, TKey> selector)
        {
            return null;
        }

        private KeyedStream<TElement, TKey> KeyBy<TKey>(Keys<TElement> keys) => new KeyedStream<TElement, TKey>(this, Clean(KeySelectorUtil.GetSelectorForKeys<TElement, TKey>(keys,
                Type, ExecutionConfig)));

        /// <summary>
        /// Method for passing user defined operators along with the type information that will transform the DataStream.
        /// </summary>
        /// <typeparam name="TOutput">type of the return stream</typeparam>
        /// <param name="operatorName">name of the operator, for logging purposes</param>
        /// <param name="outTypeInfo">the output type of the operator</param>
        /// <param name="operator">the object containing the transformation logic</param>
        /// <returns>type of the return stream</returns>
        public SingleOutputStreamOperator<TOutput> Transform<TOutput>(
            string operatorName,
            TypeInformation<TOutput> outTypeInfo,
            IOneInputStreamOperator<TElement, TOutput> @operator)
        {
            var resultTransform = new OneInputTransformation<TElement, TOutput>(Transformation, operatorName, @operator, outTypeInfo, ExecutionEnvironment.Parallelism);
            var returnStream = new SingleOutputStreamOperator<TOutput>(ExecutionEnvironment, resultTransform);

            ExecutionEnvironment.AddOperator(resultTransform);

            return returnStream;
        }

        #region [ Global Window Utilities ]

        /// <summary>
        /// Windows this <see cref="DataStream{TElement}"/> into sliding time windows.
        /// Note: This operation is inherently non-parallel since all elements have to pass through the same operator instance.
        /// </summary>
        /// <param name="size">The size of the window.</param>
        /// <returns></returns>
        public AllWindowedStream<TElement, TimeWindow> TimeWindowAll(TimeSpan size) =>
            ExecutionEnvironment.TimeCharacteristic == TimeCharacteristic.ProcessingTime
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
            ExecutionEnvironment.TimeCharacteristic == TimeCharacteristic.ProcessingTime
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

        #region [ Co-datastream Utilities ]

        /// <summary>
        /// Creates a new <see cref="ConnectedStreams{TInput1,TInput2}"/> by connecting <see cref="DataStream{TElement}"/> outputs of (possible) different types with each other.
        /// The DataStreams connected using this operator can be used with CoFunctions to apply joint transformations.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="dataStream">The DataStream with which this stream will be connected.</param>
        /// <returns>The <see cref="ConnectedStreams{TInput1,TInput2}"/>.</returns>
        public ConnectedStreams<TElement, TOutput> Connect<TOutput>(DataStream<TOutput> dataStream) =>
            new ConnectedStreams<TElement, TOutput>(ExecutionEnvironment, this, dataStream);

        /// <summary>
        /// Creates a new <see cref=""/> by connecting the current
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="broadcastStream">The broadcast stream with the broadcast state to be connected with this stream.</param>
        /// <returns>The <see cref="BroadcastConnectedStream{TInput1,TInput2}"/>.</returns>
        public BroadcastConnectedStream<TElement, TOutput> Connect<TOutput>(BroadcastStream<TOutput> broadcastStream) =>
            new BroadcastConnectedStream<TElement, TOutput>(
                ExecutionEnvironment,
                this,
                Preconditions.CheckNotNull(broadcastStream),
                broadcastStream.BroadcastStateDescriptors);

        /// <summary>
        /// Sets the partitioning of the <see cref="DataStream{TElement}"/> so that the output elements are broadcasted to every parallel instance of the next operation.
        /// </summary>
        /// <returns>The DataStream with broadcast partitioning set.</returns>
        public DataStream<TElement> Broadcast()
        {
            return null;
        }

        /// <summary>
        /// Sets the partitioning of the <see cref="DataStream{TElement}"/> so that the output elements are broadcasted to every parallel instance of the next operation. In addition, it implicitly as many <see cref="IBroadcastState{TKey,TValue}"/> as the specified descriptors which can be used to store the element of the stream.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="broadcastStateDescriptors">the descriptors of the broadcast states to create.</param>
        /// <returns>A <see cref="BroadcastStream{T}"/> which can be used in the <see cref="Connect{TOutput}(FLink.Streaming.Api.DataStreams.DataStream{TOutput})}"/> to create a <see cref="BroadcastConnectedStream{TInput1,TInput2}"/> for further processing of the elements.</returns>
        public BroadcastStream<TElement> Broadcast<TKey, TValue>(params MapStateDescriptor<TKey, TValue>[] broadcastStateDescriptors)
        {
            return null;
        }

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
