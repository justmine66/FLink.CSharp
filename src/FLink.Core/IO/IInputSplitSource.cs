using System;
using FLink.Core.Api.Common.IO;

namespace FLink.Core.IO
{
    /// <summary>
    /// InputSplitSources create <see cref="IInputSplit"/>s that define portions of data to be produced by <see cref="IInputFormat{OT,T}"/>s.
    /// </summary>
    /// <typeparam name="T">The type of the input splits created by the source.</typeparam>
    public interface IInputSplitSource<T> where T : IInputSplit
    {
        /// <summary>
        /// Computes the input splits. The given minimum number of splits is a hint as to how many splits are desired.
        /// </summary>
        /// <param name="minNumSplits">Number of minimal input splits, as a hint.</param>
        /// <returns>An array of input splits.</returns>
        /// <exception cref="Exception">Exceptions when creating the input splits may be forwarded and will cause the execution to permanently fail.</exception>
        T[] CreateInputSplits(int minNumSplits);

        /// <summary>
        /// Returns the assigner for the input splits. Assigner determines which parallel instance of the input format gets which input split.
        /// </summary>
        /// <param name="inputSplits">The input split assigner.</param>
        /// <returns></returns>
        IInputSplitAssigner GetInputSplitAssigner(T[] inputSplits);
    }
}
