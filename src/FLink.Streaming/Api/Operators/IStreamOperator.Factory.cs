using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Streaming.Api.Graphs;
using FLink.Streaming.Runtime.StreamRecords;
using FLink.Streaming.Runtime.Tasks;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A factory to create <see cref="IStreamOperator{TOutput}"/>
    /// </summary>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public interface IStreamOperatorFactory<TOutput>
    {
        /// <summary>
        /// Create the operator. Sets access to the context and the output.
        /// </summary>
        /// <typeparam name="TOperator"></typeparam>
        /// <param name="containingTask"></param>
        /// <param name="config"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        TOperator CreateStreamOperator<TOperator>(
            StreamTask<TOutput, TOperator> containingTask,
            StreamConfig config,
            IOutput<StreamRecord<TOutput>> output)
            where TOperator : IStreamOperator<TOutput>;

        /// <summary>
        /// Gets and sets the chaining strategy for operator factory.
        /// </summary>
        ChainingStrategy ChainingStrategy { get; set; }

        /// <summary>
        /// Is this factory for <see cref="StreamSource{TOut,TSrc}"/>.
        /// </summary>
        bool IsStreamSource { get; }

        /// <summary>
        /// If the stream operator need access to the output type information at <see cref="StreamGraph"/> generation.
        /// This can be useful for cases where the output type is specified by the returns method and, thus, after the stream operator has been created.
        /// </summary>
        bool IsOutputTypeConfigurable { get; }

        /// <summary>
        /// Is called by the <see cref="StreamGraphGenerator"/> method when the <see cref="StreamGraph"/> is generated.
        /// The method is called with the output <see cref="TypeInformation{TType}"/> which is also used for the <see cref="StreamTask{TOutput,TOperator}"/> output serializer.
        /// </summary>
        /// <param name="type">Output type information of the <see cref="StreamTask{TOutput,TOperator}"/></param>
        /// <param name="executionConfig">Execution configuration</param>
        void SetOutputType(TypeInformation<TOutput> type, ExecutionConfig executionConfig);

        /// <summary>
        /// If the stream operator need to be configured with the data type they will operate on.
        /// </summary>
        bool IsInputTypeConfigurable { get; }

        /// <summary>
        /// Is called by the <see cref="StreamGraph.AddOperator{TIn, TOut}"/> method when the <see cref="StreamGraph"/> is generated.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="type"></param>
        /// <param name="executionConfig"></param>
        void SetInputType<TInput>(TypeInformation<TInput> type, ExecutionConfig executionConfig);
    }
}
