using FLink.Runtime.Execution;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Runtime.Tasks
{
    /// <summary>
    /// <see cref="StreamTask{TOutput,TOperator}"/> for executing a <see cref="StreamSource{TOut,TSrc}"/>.
    /// </summary>
    /// <typeparam name="TOutput">Type of the output elements of this source.</typeparam>
    /// <typeparam name="TFunction">Type of the source function for the stream source operator</typeparam>
    /// <typeparam name="TOperator">Type of the stream source operator</typeparam>
    public class SourceStreamTask<TOutput, TFunction, TOperator> : StreamTask<TOutput, TOperator>
        where TFunction : ISourceFunction<TOutput>
        where TOperator : StreamSource<TOutput, TFunction>
    {
        public SourceStreamTask(IEnvironment environment) : base(environment)
        {
        }

        public override void Invoke()
        {
            throw new System.NotImplementedException();
        }
    }
}
