using System;
using System.Collections.Generic;
using System.Text;

namespace FLink.Core.Api.Common
{
    /// <summary>
    /// The execution mode specifies how a batch program is executed in terms of data exchange: pipelining or batched.
    /// </summary>
    public enum ExecutionMode
    {
        /// <summary>
        /// Executes the program in a pipelined fashion (including shuffles and broadcasts), except for data exchanges that are susceptible to deadlocks when pipelining. These data exchanges are performed in a batch manner.
        /// </summary>
        Pipelined,
        PipelinedForced,
        Batch,
        BatchForced
    }
}
