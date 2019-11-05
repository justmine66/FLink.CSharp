using System;
using FLink.Core.Api.Common;
using FLink.Runtime.Execution;
using FLink.Runtime.Query;

namespace FLink.Runtime.Test.Operators.Utils
{
    public class DummyEnvironment : IEnvironment
    {
        public ExecutionConfig ExecutionConfig { get; }
        public JobId JobId { get; }
        public Type UserClassType { get; }
        public TaskKvStateRegistry TaskKvStateRegistry { get; }
    }
}
