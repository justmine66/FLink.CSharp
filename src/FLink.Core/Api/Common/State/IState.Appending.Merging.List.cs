using System.Collections.Generic;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="IState"/> interface for partitioned list state in Operations.
    /// The state is accessed and modified by user functions, and checkpointed consistently by the system as part of the distributed snapshots.
    /// </summary>
    /// <typeparam name="TValue">Type of values that this list state keeps.</typeparam>
    public interface IListState<TValue> : IMergingState<TValue, IEnumerable<TValue>>
    {
        /// <summary>
        /// Updates the operator state accessible by <see cref="IAppendingState{TIn,TOut}.Get()"/> to the given list of values. The next time <see cref="IAppendingState{TIn,TOut}.Get()"/> is called (for the same state partition) the returned state will represent the updated list.
        /// If null or an empty list is passed in, the state value will be null.
        /// </summary>
        /// <param name="values">The new values for the state.</param>
        /// <exception cref="System.Exception">The method may forward exception thrown internally (by I/O or functions).</exception>
        void Update(List<TValue> values);

        /// <summary>
        /// adding the given values to existing list of values by <see cref="IAppendingState{TIn,TOut}.Get()"/>. The next time <see cref="IAppendingState{TIn,TOut}.Get()"/> is called (for the same state partition) the returned state will represent the updated list.
        /// If null or an empty list is passed in, the state value will be null.
        /// </summary>
        /// <param name="values">The new values for the state.</param>
        /// <exception cref="System.Exception">The method may forward exception thrown internally (by I/O or functions).</exception>
        void AddAll(List<TValue> values);
    }
}
