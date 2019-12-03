using FLink.Core.IO;

namespace FLink.Runtime.JobGraphs.Tasks
{
    /// <summary>
    /// An input split provider can be successively queried to provide a series of <see cref="IInputSplit"/> objects a task is supposed to consume in the course of its execution.
    /// </summary>
    public interface IInputSplitProvider
    {
        /// <summary>
        /// Gets the next input split to be consumed by the calling task.
        /// </summary>
        IInputSplit NextInputSplit { get; }
    }
}
