using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FLink.Core.Api.Common;
using FLink.Core.Configurations;
using FLink.Runtime.Execution;
using FLink.Runtime.JobGraphs.Tasks;
using FLink.Runtime.Query;
using FLink.Runtime.TaskExecutors;

namespace FLink.Runtime.Test.Operators.Utils
{
    public class DummyEnvironment : IEnvironment
    {
        public ExecutionConfig ExecutionConfig { get; }
        public JobId JobId { get; }
        public Type UserClassType { get; }
        public TaskKvStateRegistry TaskKvStateRegistry { get; }
        public TaskInfo TaskInfo { get; }
        public Configuration TaskConfiguration { get; }
        public Configuration JobConfiguration { get; }
        public IDictionary<string, TaskCompletionSource<string>> DistributedCacheEntries { get; }
        public IInputSplitProvider InputSplitProvider { get; }
        public IGlobalAggregateManager GlobalAggregateManager { get; }
    }
}
