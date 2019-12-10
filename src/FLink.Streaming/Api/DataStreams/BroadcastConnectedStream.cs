using System.Collections.Generic;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Util;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions.Co;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// A BroadcastConnectedStream represents the result of connecting a keyed or non-keyed stream.
    /// </summary>
    /// <typeparam name="TInput1">The input type of the non-broadcast side.</typeparam>
    /// <typeparam name="TInput2">The input type of the broadcast side.</typeparam>
    public class BroadcastConnectedStream<TInput1, TInput2>
    {
        public StreamExecutionEnvironment ExecutionEnvironment;

        /// <summary>
        /// Returns the non-broadcast <see cref="DataStream{TElement}"/>.
        /// The stream which, by convention, is not broadcasted.
        /// </summary>
        public DataStream<TInput1> FirstInput { get; }

        /// <summary>
        /// Returns the <see cref="BroadcastStream{T}"/>.
        /// The stream which, by convention, is the broadcast one.
        /// </summary>
        public BroadcastStream<TInput2> SecondInput { get; }

        /// <summary>
        /// Gets the type of the first input.
        /// </summary>
        public TypeInformation<TInput1> Type1 => FirstInput.Type;

        /// <summary>
        /// Gets the type of the second input.
        /// </summary>
        public TypeInformation<TInput2> Type2 => SecondInput.Type;

        public IList<MapStateDescriptor<dynamic, dynamic>> BroadcastStateDescriptors;

        public BroadcastConnectedStream(
            StreamExecutionEnvironment env,
            DataStream<TInput1> input1,
            BroadcastStream<TInput2> input2,
            List<MapStateDescriptor<dynamic, dynamic>> broadcastStateDescriptors)
        {
            ExecutionEnvironment = Preconditions.CheckNotNull(env);
            FirstInput = Preconditions.CheckNotNull(input1);
            SecondInput = Preconditions.CheckNotNull(input2);
            BroadcastStateDescriptors = Preconditions.CheckNotNull(broadcastStateDescriptors);
        }

        /// <summary>
        /// Assumes as inputs a <see cref="BroadcastStream{TElement}"/> and a <see cref="KeyedStream{TElement,TKey}"/> and applies the given <see cref="KeyedBroadcastProcessFunction{TKey,TInput1,TInput2,TOutput}"/> on them, thereby creating a transformed output stream.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the keyed stream.</typeparam>
        /// <typeparam name="TOutput">The type of the output elements.</typeparam>
        /// <param name="function">The <see cref="KeyedBroadcastProcessFunction{TKey,TInput1,TInput2,TOutput}"/> that is called for each element in the stream.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Process<TKey, TOutput>(KeyedBroadcastProcessFunction<TKey, TInput1, TInput2, TOutput> function)
        {
            return null;
        }

        /// <summary>
        /// Assumes as inputs a <see cref="BroadcastStream{TElement}"/> and a <see cref="KeyedStream{TElement,TKey}"/> and applies the given <see cref="KeyedBroadcastProcessFunction{TKey,TInput1,TInput2,TOutput}"/> on them, thereby creating a transformed output stream.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the keyed stream.</typeparam>
        /// <typeparam name="TOutput">The type of the output elements.</typeparam>
        /// <param name="function">The <see cref="KeyedBroadcastProcessFunction{TKey,TInput1,TInput2,TOutput}"/> that is called for each element in the stream.</param>
        /// <param name="outTypeInfo">The type of the output elements.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Process<TKey, TOutput>(KeyedBroadcastProcessFunction<TKey, TInput1, TInput2, TOutput> function, TypeInformation<TOutput> outTypeInfo)
        {
            return null;
        }

        /// <summary>
        /// Assumes as inputs a {@link BroadcastStream} and a non-keyed <see cref="DataStream{TElement}"/> and applies the given <see cref="BroadcastProcessFunction{TInput1,TInput2,TOutput}"/> on them, thereby creating a transformed output stream.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output elements.</typeparam>
        /// <param name="function">The <see cref="BroadcastProcessFunction{TInput1,TInput2,TOutput}"/> that is called for each element in the stream.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Process<TOutput>(BroadcastProcessFunction<TInput1, TInput2, TOutput> function)
        {
            return null;
        }

        /// <summary>
        /// Assumes as inputs a {@link BroadcastStream} and a non-keyed <see cref="DataStream{TElement}"/> and applies the given <see cref="BroadcastProcessFunction{TInput1,TInput2,TOutput}"/> on them, thereby creating a transformed output stream.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output elements.</typeparam>
        /// <param name="function">The <see cref="BroadcastProcessFunction{TInput1,TInput2,TOutput}"/> that is called for each element in the stream.</param>
        /// <param name="outTypeInfo">The type of the output elements.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Process<TOutput>(BroadcastProcessFunction<TInput1, TInput2, TOutput> function, TypeInformation<TOutput> outTypeInfo)
        {
            return null;
        }

        public SingleOutputStreamOperator<TOutput> Transform<TOutput>(string functionName, TypeInformation<TOutput> outTypeInfo, ITwoInputStreamOperator<TInput1, TInput2, TOutput> @operator)
        {
            return null;
        }
    }
}
