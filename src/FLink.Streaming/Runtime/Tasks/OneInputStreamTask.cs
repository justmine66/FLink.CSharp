using FLink.Runtime.Execution;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Runtime.Tasks
{
    /// <summary>
    /// A <see cref="StreamTask{TOutput,TOperator}"/> for executing a <see cref="OneInputStreamTask{IN,OUT}"/>.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class OneInputStreamTask<TInput, TOutput> : StreamTask<TOutput, IOneInputStreamOperator<TInput, TOutput>>
    {
        public OneInputStreamTask(IEnvironment environment) : base(environment)
        {
        }

        public override void Invoke()
        {
            throw new System.NotImplementedException();
        }
    }
}
