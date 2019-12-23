using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.CSharp.Functions;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Functions.Co;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// ConnectedStreams represent two connected streams of (possibly) different data types.
    /// Connected streams are useful for cases where operations on one stream directly affect the operations on the other stream, usually via shared state between the streams.
    /// An example for the use of connected streams would be to apply rules that change over time onto another stream. One of the connected streams has the rules, the other stream the elements to apply the rules to. The operation on the connected stream maintains the current set of rules in the state. It may receive either a rule update and update the state or a data element and apply the rules in the state to the element.
    /// The connected stream can be conceptually viewed as a union stream of an Either type, that holds either the first stream's type or the second stream's type.
    /// </summary>
    /// <typeparam name="TInput1">Type of the first input data steam.</typeparam>
    /// <typeparam name="TInput2">Type of the second input data stream.</typeparam>
    public class ConnectedStreams<TInput1, TInput2>
    {
        public ConnectedStreams(StreamExecutionEnvironment environment, DataStream<TInput1> inputStream1, DataStream<TInput2> inputStream2)
        {
            Environment = environment;
            InputStream1 = inputStream1;
            InputStream2 = inputStream2;
        }

        public StreamExecutionEnvironment Environment { get; }

        /// <summary>
        /// Returns the first <see cref="DataStream{TElement}"/>.
        /// </summary>
        public DataStream<TInput1> InputStream1 { get; }

        /// <summary>
        /// Returns the second <see cref="DataStream{TElement}"/>.
        /// </summary>
        public DataStream<TInput2> InputStream2 { get; }

        /// <summary>
        /// Gets the type of the first input.
        /// </summary>
        public TypeInformation<TInput1> Type1 => InputStream1.Type;

        /// <summary>
        /// Gets the type of the second input.
        /// </summary>
        public TypeInformation<TInput2> Type2 => InputStream2.Type;

        /// <summary>
        /// KeyBy operation for connected data stream. Assigns keys to the elements of input1 and input2 according to keyPosition1 and keyPosition2.
        /// </summary>
        /// <param name="keyPosition1">The field used to compute the hashcode of the elements in the first input stream.</param>
        /// <param name="keyPosition2">The field used to compute the hashcode of the elements in the second input stream.</param>
        /// <returns>The grouped <see cref="ConnectedStreams{IN1,IN2}"/></returns>
        public ConnectedStreams<TInput1, TInput2> KeyBy(int keyPosition1, int keyPosition2) =>
            new ConnectedStreams<TInput1, TInput2>(Environment, InputStream1.KeyBy(keyPosition1),
                InputStream2.KeyBy(keyPosition2));

        /// <summary>
        /// KeyBy operation for connected data stream. Assigns keys to the elements of input1 and input2 according to keyPositions1 and keyPositions2.
        /// </summary>
        /// <param name="keyPositions1">The fields used to group the first input stream.</param>
        /// <param name="keyPositions2">The fields used to group the second input stream.</param>
        /// <returns>The grouped <see cref="ConnectedStreams{IN1,IN2}"/></returns>
        public ConnectedStreams<TInput1, TInput2> KeyBy(int[] keyPositions1, int[] keyPositions2) =>
            new ConnectedStreams<TInput1, TInput2>(Environment, InputStream1.KeyBy(keyPositions1),
                InputStream2.KeyBy(keyPositions2));

        /// <summary>
        /// KeyBy operation for connected data stream using key expressions. Assigns keys to the elements of input1 and input2 according to field1 and field2. A field expression is either the name of a public field or a getter method with parentheses of the <see cref="DataStream{TElement}"/>S underlying type. A dot can be used to drill down into objects.
        /// </summary>
        /// <param name="field1">The grouping expression for the first input</param>
        /// <param name="field2">The grouping expression for the second input</param>
        /// <returns>The grouped <see cref="ConnectedStreams{IN1,IN2}"/></returns>
        public ConnectedStreams<TInput1, TInput2> KeyBy(string field1, string field2) => new ConnectedStreams<TInput1, TInput2>(
            Environment, InputStream1.KeyBy(field1),
            InputStream2.KeyBy(field2));

        /// <summary>
        /// KeyBy operation for connected data stream using key expressions. Assigns keys to the elements of input1 and input2 according to field1 and field2. A field expression is either the name of a public field or a getter method with parentheses of the <see cref="DataStream{TElement}"/>S underlying type. A dot can be used to drill down into objects.
        /// </summary>
        /// <param name="field1">The grouping expression for the first input</param>
        /// <param name="field2">The grouping expression for the second input</param>
        /// <returns>The grouped <see cref="ConnectedStreams{IN1,IN2}"/></returns>
        public ConnectedStreams<TInput1, TInput2> KeyBy(string[] field1, string[] field2) => new ConnectedStreams<TInput1, TInput2>(
            Environment, InputStream1.KeyBy(field1),
            InputStream2.KeyBy(field2));

        /// <summary>
        /// KeyBy operation for connected data stream. Assigns keys to the elements of input1 and input2 using keySelector1 and keySelector2.
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="keySelector1">The <see cref="IKeySelector{TObject,TKey}"/> used for grouping the first input</param>
        /// <param name="keySelector2">The <see cref="IKeySelector{TObject,TKey}"/> used for grouping the second input</param>
        /// <returns>The partitioned <see cref="ConnectedStreams{IN1,IN2}"/></returns>
        public ConnectedStreams<TInput1, TInput2> KeyBy<TKey1, TKey2>(IKeySelector<TInput1, TKey1> keySelector1,
            IKeySelector<TInput2, TKey2> keySelector2) => new ConnectedStreams<TInput1, TInput2>(Environment,
            InputStream1.KeyBy(keySelector1), InputStream2.KeyBy(keySelector2));

        /// <summary>
        /// Applies a CoMap transformation on a <see cref="ConnectedStreams{IN1,IN2}"/> and maps the output to a common type. The transformation calls a <see cref="ICoMapFunction{TInput1,TInput2,TOutput}.Map1"/> for each element of the first input and <see cref="ICoMapFunction{TInput1,TInput2,TOutput}.Map2"/> for each element of the second input.
        /// Each CoMapFunction call returns exactly one element.
        /// </summary>
        /// <param name="coMapper">The CoMapFunction used to jointly transform the two input DataStreams</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/></returns>
        public SingleOutputStreamOperator<TOutput> Map<TOutput>(ICoMapFunction<TInput1, TInput2, TOutput> coMapper)
        {
            return null;
        }

        /// <summary>
        /// Applies a CoMap transformation on a <see cref="ConnectedStreams{IN1,IN2}"/> and maps the output to a common type. The transformation calls a <see cref="ICoMapFunction{TInput1,TInput2,TOutput}.Map1"/> for each element of the first input and <see cref="ICoMapFunction{TInput1,TInput2,TOutput}.Map2"/> for each element of the second input.
        /// Each CoMapFunction call returns exactly one element.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="coMapper">The CoMapFunction used to jointly transform the two input DataStreams</param>
        /// <param name="outputType"><see cref="TypeInformation{TType}"/> for the result type of the function.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/></returns>
        public SingleOutputStreamOperator<TOutput> Map<TOutput>(ICoMapFunction<TInput1, TInput2, TOutput> coMapper, TypeInformation<TOutput> outputType)
        {
            return null;
        }

        /// <summary>
        /// Applies a CoFlatMap transformation on a <see cref="ConnectedStreams{IN1,IN2}"/> and maps the output to a common type. The transformation calls a <see cref="ICoMapFunction{TInput1,TInput2,TOutput}.Map1"/> for each element of the first input and <see cref="ICoMapFunction{TInput1,TInput2,TOutput}.Map2"/> for each element of the second input.
        /// Each CoMapFunction call returns exactly one element.
        /// </summary>
        /// <param name="coFlatMapper">The CoMapFunction used to jointly transform the two input DataStreams</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/></returns>
        public SingleOutputStreamOperator<TOutput> FlatMap<TOutput>(ICoFlatMapFunction<TInput1, TInput2, TOutput> coFlatMapper)
        {
            return null;
        }

        /// <summary>
        /// Applies a CoFlatMap transformation on a <see cref="ConnectedStreams{IN1,IN2}"/> and maps the output to a common type. The transformation calls a <see cref="ICoMapFunction{TInput1,TInput2,TOutput}.Map1"/> for each element of the first input and <see cref="ICoMapFunction{TInput1,TInput2,TOutput}.Map2"/> for each element of the second input.
        /// Each CoMapFunction call returns exactly one element.
        /// </summary>
        /// <param name="coFlatMapper">The CoMapFunction used to jointly transform the two input DataStreams</param>
        /// <param name="outputType"><see cref="TypeInformation{TType}"/> for the result type of the function.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/></returns>
        public SingleOutputStreamOperator<TOutput> FlatMap<TOutput>(ICoFlatMapFunction<TInput1, TInput2, TOutput> coFlatMapper,
            TypeInformation<TOutput> outputType)
        {
            return null;
        }

        /// <summary>
        /// Applies the given <see cref="CoProcessFunction{TInput1,TInput2,TOutput}"/> on the connected input streams, thereby creating a transformed output stream.
        /// The function will be called for every element in the input streams and can produce zero or more output elements. Contrary to the <see cref="ICoFlatMapFunction{TInput1,TInput2,TOutput}"/>} function, this function can also query the time and set timers. When reacting to the firing of set timers the function can directly emit elements and/or register yet more timers.
        /// </summary>
        /// <typeparam name="TOutput">The type of elements emitted by the <see cref="CoProcessFunction{TInput1,TInput2,TOutput}"/>.</typeparam>
        /// <param name="coProcessFunction">The <see cref="CoProcessFunction{TInput1,TInput2,TOutput}"/>} that is called for each element in the stream.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Process<TOutput>(CoProcessFunction<TInput1, TInput2, TOutput> coProcessFunction)
        {
            return null;
        }

        /// <summary>
        /// Applies the given <see cref="CoProcessFunction{TInput1,TInput2,TOutput}"/> on the connected input streams, thereby creating a transformed output stream.
        /// The function will be called for every element in the input streams and can produce zero or more output elements. Contrary to the <see cref="ICoFlatMapFunction{TInput1,TInput2,TOutput}"/>} function, this function can also query the time and set timers. When reacting to the firing of set timers the function can directly emit elements and/or register yet more timers.
        /// </summary>
        /// <typeparam name="TOutput">The type of elements emitted by the <see cref="CoProcessFunction{TInput1,TInput2,TOutput}"/>.</typeparam>
        /// <param name="coProcessFunction">The <see cref="CoProcessFunction{TInput1,TInput2,TOutput}"/>} that is called for each element in the stream.</param>
        /// <param name="outputType">The type of elements emitted by the <see cref="CoProcessFunction{TInput1,TInput2,TOutput}"/>.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Process<TOutput>(CoProcessFunction<TInput1, TInput2, TOutput> coProcessFunction, TypeInformation<TOutput> outputType)
        {
            return null;
        }

        /// <summary>
        /// Applies the given <see cref="KeyedCoProcessFunction{TKey,TInput1,TInput2,TOutput}"/> on the connected input streams, thereby creating a transformed output stream.
        /// The function will be called for every element in the input streams and can produce zero or more output elements. Contrary to the <see cref="ICoFlatMapFunction{TInput1,TInput2,TOutput}"/>} function, this function can also query the time and set timers. When reacting to the firing of set timers the function can directly emit elements and/or register yet more timers.
        /// </summary>
        /// <typeparam name="TOutput">The type of elements emitted by the <see cref="KeyedCoProcessFunction{TKey,TInput1,TInput2,TOutput}"/>.</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keyedCoProcessFunction">The <see cref="KeyedCoProcessFunction{TKey,TInput1,TInput2,TOutput}"/>} that is called for each element in the stream.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Process<TKey, TOutput>(KeyedCoProcessFunction<TKey, TInput1, TInput2, TOutput> keyedCoProcessFunction)
        {
            return null;
        }

        /// <summary>
        /// Applies the given <see cref="KeyedCoProcessFunction{TKey,TInput1,TInput2,TOutput}"/> on the connected input streams, thereby creating a transformed output stream.
        /// The function will be called for every element in the input streams and can produce zero or more output elements. Contrary to the <see cref="ICoFlatMapFunction{TInput1,TInput2,TOutput}"/>} function, this function can also query the time and set timers. When reacting to the firing of set timers the function can directly emit elements and/or register yet more timers.
        /// </summary>
        /// <typeparam name="TOutput">The type of elements emitted by the <see cref="KeyedCoProcessFunction{TKey,TInput1,TInput2,TOutput}"/>.</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keyedCoProcessFunction">The <see cref="KeyedCoProcessFunction{TKey,TInput1,TInput2,TOutput}"/>} that is called for each element in the stream.</param>
        /// <param name="outputType">The type of elements emitted by the <see cref="KeyedCoProcessFunction{TKey,TInput1,TInput2,TOutput}"/>.</param>
        /// <returns>The transformed <see cref="DataStream{TElement}"/>.</returns>
        public SingleOutputStreamOperator<TOutput> Process<TKey, TOutput>(KeyedCoProcessFunction<TKey, TInput1, TInput2, TOutput> keyedCoProcessFunction, TypeInformation<TOutput> outputType)
        {
            return null;
        }

        public SingleOutputStreamOperator<R> Transform<R>(string functionName, TypeInformation<R> outTypeInfo, ITwoInputStreamOperator<TInput1, TInput2, R> @operator)
        {
            return null;
        }
    }
}
