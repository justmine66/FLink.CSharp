using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Core.Util;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// A DataStream represents a stream of elements of the same type.
    /// A DataStream can be transformed into another DataStream by applying a transformation as for example:
    /// </summary>
    /// <typeparam name="T">The type of the elements in this stream.</typeparam>
    public class DataStream<T>
    {
        public StreamExecutionEnvironment Environment { get; }

        public Transformation<T> Transformation { get; }

        public DataStream(StreamExecutionEnvironment environment, Transformation<T> transformation)
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
        public SingleOutputStreamOperator<TOutput> FlatMap<TOutput>(IFlatMapFunction<T, TOutput> flatMapper)
        {
            return null;
        }

        /// <summary>
        /// Partitions the operator state of a <see cref="DataStream{T}"/> using field expressions. A field expression is either the name of a public field or a getter method with parentheses of the <see cref="DataStream{T}"/>'s underlying type.
        /// </summary>
        /// <param name="fields">One or more field expressions on which the state of the <see cref="DataStream{T}"/> operators will be partitioned.</param>
        /// <returns>The <see cref="DataStream{T}"/> with partitioned state (i.e. KeyedStream)</returns>
        public KeyedStream<T, object> KeyBy(params string[] fields)
        {
            return null;
        }

        /// <summary>
        /// Writes a DataStream to the standard output stream (stdout).
        /// For each element of the DataStream the result of <see cref="object.ToString()"/> is written.
        /// NOTE: This will print to stdout on the machine where the code is executed, i.e. the FLink worker.
        /// </summary>
        /// <returns>The closed DataStream.</returns>
        public DataStreamSink<T> Print()
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
