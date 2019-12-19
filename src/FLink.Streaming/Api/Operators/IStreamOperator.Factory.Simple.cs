using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Util;
using FLink.Streaming.Api.Graphs;
using FLink.Streaming.Runtime.StreamRecords;
using FLink.Streaming.Runtime.Tasks;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Simple factory which just wrap existed <see cref="IStreamOperator{TOutput}"/>.
    /// </summary>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public class SimpleOperatorFactory<TOutput> : IStreamOperatorFactory<TOutput>
    {
        /// <summary>
        /// Create a SimpleOperatorFactory from existed StreamOperator.
        /// </summary>
        /// <param name="operator"></param>
        /// <returns></returns>
        public static SimpleOperatorFactory<TOutput> Of(IStreamOperator<TOutput> @operator)
        {
            return default;
        }

        public TOperator CreateStreamOperator<TOperator>(StreamTask<TOutput, TOperator> containingTask, StreamConfig config, IOutput<StreamRecord<TOutput>> output) where TOperator : IStreamOperator<TOutput>
        {
            throw new System.NotImplementedException();
        }

        public SimpleOperatorFactory(IStreamOperator<TOutput> @operator)
        {
            Operator = Preconditions.CheckNotNull(@operator);
        }

        public IStreamOperator<TOutput> Operator { get; }

        public ChainingStrategy ChainingStrategy
        {
            get => Operator.ChainingStrategy;
            set => Operator.ChainingStrategy = value;
        }

        public bool IsStreamSource { get; }

        public bool IsOutputTypeConfigurable { get; }

        public void SetOutputType(TypeInformation<TOutput> type, ExecutionConfig executionConfig)
        {
            throw new System.NotImplementedException();
        }

        public bool IsInputTypeConfigurable { get; }

        public void SetInputType<TInput>(TypeInformation<TInput> type, ExecutionConfig executionConfig)
        {
            throw new System.NotImplementedException();
        }
    }
}
