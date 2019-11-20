using FLink.Core.Api.Common;
using FLink.Core.Configurations;
using FLink.Extensions.CSharp;
using FLink.Streaming.Api.Graph;

namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// A special <see cref="StreamExecutionEnvironment"/> that is used in the web frontend when generating a user-inspectable graph of a streaming job.
    /// </summary>
    public class StreamPlanEnvironment : StreamExecutionEnvironment
    {
        private readonly ExecutionEnvironment _env;

        public StreamPlanEnvironment(ExecutionEnvironment env)
        {
            _env = env;

            var parallelism = env.Parallelism;
            SetParallelism(parallelism > 0
                ? parallelism
                : GlobalConfiguration.LoadConfiguration().GetInt(CoreOptions.DefaultParallelism));
        }

        public override JobExecutionResult Execute(StreamGraph streamGraph)
        {
            throw new System.NotImplementedException();
        }
    }
}
