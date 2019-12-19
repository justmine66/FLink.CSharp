using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Graphs;
using FLink.Streaming.Api.Transformations;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Partitioners;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// A <see cref="KeyedStream{TElement,TKey}"/> represents a <see cref="DataStream{TElement}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the Keyed Stream.</typeparam>
    /// <typeparam name="TKey">The type of the key in the Keyed Stream.</typeparam>
    public class KeyedStream<TElement, TKey> : DataStream<TElement>
    {
        /// <summary>
        /// The key selector that can get the key by which the stream if partitioned from the elements.
        /// </summary>
        public IKeySelector<TElement, TKey> KeySelector { get; }

        /// <summary>
        /// The type of the key by which the stream is partitioned.
        /// </summary>
        public TypeInformation<TKey> KeyType { get; }

        /// <summary>
        /// Creates a new <see cref="KeyedStream{TElement,TKey}"/> using the given <see cref="IKeySelector{TObject,TKey}"/> to partition operator state by key.
        /// </summary>
        /// <param name="stream">Base stream of data</param>
        /// <param name="selector">Function for determining state partitions</param>
        public KeyedStream(DataStream<TElement> stream, IKeySelector<TElement, TKey> selector)
            : this(stream, selector, TypeExtractor.GetKeySelectorTypes(selector, stream.Type))
        {
        }

        /// <summary>
        /// Creates a new <see cref="KeyedStream{TElement,TKey}"/> using the given <see cref="IKeySelector{TObject,TKey}"/> and <see cref="TypeInformation{TType}"/> to partition operator state by key, where the partitioning is defined by a <see cref="PartitionTransformation{TElement}"/>.
        /// </summary>
        /// <param name="stream">Base stream of data</param>
        /// <param name="selector">Function for determining state partitions</param>
        /// <param name="keyType">Defines the type of the extracted keys</param>
        public KeyedStream(DataStream<TElement> stream, IKeySelector<TElement, TKey> selector, TypeInformation<TKey> keyType)
            : this(
                stream,
                new PartitionTransformation<TElement>(
                    stream.Transformation,
                    new KeyGroupStreamPartitioner<TElement, TKey>(selector, StreamGraphGenerator.DefaultLowerBoundMaxParallelism)),
                selector,
                keyType)
        {
        }

        /// <summary>
        /// Creates a new <see cref="KeyedStream{TElement,TKey}"/> using the given <see cref="IKeySelector{TObject,TKey}"/> and <see cref="TypeInformation{TType}"/> to partition operator state by key, where the partitioning is defined by a <see cref="PartitionTransformation{TElement}"/>.
        /// </summary>
        /// <param name="stream">Base stream of data</param>
        /// <param name="transformation">Function that determines how the keys are distributed to downstream operator(s)</param>
        /// <param name="selector">Function to extract keys from the base stream</param>
        /// <param name="keyType">Defines the type of the extracted keys</param>
        internal KeyedStream(
            DataStream<TElement> stream,
            PartitionTransformation<TElement> transformation,
            IKeySelector<TElement, TKey> selector,
            TypeInformation<TKey> keyType)
            : base(stream.ExecutionEnvironment, transformation)
        {
            KeySelector = Clean(selector);
            KeyType = ValidateKeyType(keyType);
        }

        /// <summary>
        /// Windows this <see cref="KeyedStream{T,TKey}"/> into tumbling time windows.
        /// </summary>
        /// <param name="size">The size of the window.</param>
        /// <returns></returns>
        public WindowedStream<TElement, TKey, TimeWindow> TimeWindow(TimeSpan size)
        {
            return ExecutionEnvironment.TimeCharacteristic == TimeCharacteristic.ProcessingTime
                ? Window(TumblingProcessingTimeWindowAssigner<TElement>.Of(size))
                : Window(TumblingEventTimeWindowAssigner<TElement>.Of(size));
        }

        /// <summary>
        /// Windows this <see cref="KeyedStream{T,TKey}"/> into sliding time windows.
        /// </summary>
        /// <param name="size">The size of the window.</param>
        /// <param name="slide">The slide interval.</param>
        /// <returns></returns>
        public WindowedStream<TElement, TKey, TimeWindow> TimeWindow(TimeSpan size, TimeSpan slide)
        {
            return ExecutionEnvironment.TimeCharacteristic == TimeCharacteristic.ProcessingTime
                ? Window(SlidingProcessingTimeWindowAssigner<TElement>.Of(size, slide))
                : Window(SlidingEventTimeWindowAssigner<TElement>.Of(size, slide));
        }

        /// <summary>
        /// Windows this KeyedStream into tumbling count windows.
        /// </summary>
        /// <param name="size">The size of the windows in number of elements.</param>
        /// <returns></returns>
        public WindowedStream<TElement, TKey, GlobalWindow> CountWindow(long size)
        {
            return null;
        }

        /// <summary>
        /// Windows this KeyedStream into sliding count windows.
        /// </summary>
        /// <param name="size">The size of the windows in number of elements.</param>
        /// <param name="slide">The slide interval in number of elements.</param>
        /// <returns></returns>
        public WindowedStream<TElement, TKey, GlobalWindow> CountWindow(long size, long slide)
        {
            return null;
        }

        /// <summary>
        /// Windows this data stream to a <see cref="WindowedStream{T,TK,TW}"/>, which evaluates windows over a key grouped stream. Elements are put into windows by a <see cref="WindowAssigner{T,TW}"/>. The grouping of elements is done both by key and by window.
        /// </summary>
        /// <typeparam name="TW"></typeparam>
        /// <param name="assigner">The WindowAssigner that assigns elements to windows.</param>
        /// <returns>The trigger windows data stream.</returns>
        public WindowedStream<TElement, TKey, TW> Window<TW>(WindowAssigner<TElement, TW> assigner)
            where TW : Window
        {
            return new WindowedStream<TElement, TKey, TW>(this, assigner);
        }

        #region [ Non-Windowed aggregation operations ]

        /// <summary>
        /// Applies a reduce transformation on the grouped data stream grouped on by the given key position. The <see cref="IReduceFunction{T}"/> will receive input values based on the key value. Only input values with the same key will go to the same reducer.
        /// </summary>
        /// <param name="reducer">The <see cref="IReduceFunction{T}"/> that will be called for every element of the input values with the same key.</param>
        /// <returns>The transformed DataStream.</returns>
        public SingleOutputStreamOperator<TElement> Reduce(IReduceFunction<TElement> reducer)
        {
            return null;
        }

        #endregion


        /// <summary>
        /// Applies the given <see cref="KeyedProcessFunction{TK,TI,TO}"/> on the input stream, thereby creating a transformed output stream.
        /// >The function will be called for every element in the input streams and can produce zero or more output elements. Contrary to the <see cref="DataStream{T}.FlatMap{T}"/> function, this function can also query the time and set timers. When reacting to the firing of set timers the function can directly emit elements and / or register yet more timers.
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="keyedProcessFunction"></param>
        /// <returns></returns>
        public SingleOutputStreamOperator<TR> Process<TR>(KeyedProcessFunction<TKey, TElement, TR> keyedProcessFunction)
        {
            return null;
        }

        public SingleOutputStreamOperator<TR> Process<TR>(
            KeyedProcessFunction<TKey, TElement, TR> keyedProcessFunction,
            TypeInformation<TR> outputType)
        {
            return null;
        }

        /// <summary>
        /// Validates that a given type of element (as encoded by the provided <see cref="TypeInformation{TType}"/>) can be used as a key in the <see cref="DataStream{TElement}.KeyBy{selector}"/> operation.  
        /// </summary>
        /// <param name="keyType"></param>
        /// <returns></returns>
        private TypeInformation<TKey> ValidateKeyType(TypeInformation<TKey> keyType)
        {
            return keyType;
        }
    }
}
