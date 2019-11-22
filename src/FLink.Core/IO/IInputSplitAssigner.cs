using System.Collections.Generic;

namespace FLink.Core.IO
{
    /// <summary>
    /// An input split assigner distributes the <see cref="IInputSplit"/>s among the instances on which a data source exists.
    /// </summary>
    public interface IInputSplitAssigner
    {
        /// <summary>
        /// Gets the next input split that shall be consumed. The consumer's host is passed as a parameter to allow localized assignments.
        /// </summary>
        /// <param name="host">The host address of split requesting task.</param>
        /// <param name="taskId">The id of the split requesting task.</param>
        /// <returns>the next input split to be consumed, or <code>null</code> if no more splits remain.</returns>
        IInputSplit GetNextInputSplit(string host, int taskId);

        /// <summary>
        /// Return the splits to assigner if the task failed to process it.
        /// </summary>
        /// <param name="splits">The list of input splits to be returned.</param>
        /// <param name="taskId">The id of the task that failed to process the input splits.</param>
        void ReturnInputSplit(IList<IInputSplit> splits, int taskId);
    }
}
