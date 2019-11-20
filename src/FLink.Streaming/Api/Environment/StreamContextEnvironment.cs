using FLink.Clients.Program;
using FLink.Core.Api.Common;
using FLink.Streaming.Api.Graph;

namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// Special <see cref="StreamExecutionEnvironment"/> that will be used in cases where the CLI client or testing utilities create a <see cref="StreamExecutionEnvironment"/> that should be used when <see cref="StreamExecutionEnvironment.GetExecutionEnvironment()"/> is called.
    /// </summary>
    public class StreamContextEnvironment : StreamExecutionEnvironment
    {
        private readonly ContextEnvironment _context;

        public StreamContextEnvironment(ContextEnvironment context)
        {
            _context = context;
            if (context.Parallelism > 0)
                SetParallelism(context.Parallelism);
        }

        public override JobExecutionResult Execute(StreamGraph streamGraph)
        {
            throw new System.NotImplementedException();
        }
    }
}
