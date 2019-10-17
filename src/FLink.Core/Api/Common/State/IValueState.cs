namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="IState"/> interface for partitioned single-value state. The value can be retrieved or updated.
    /// The state is accessed and modified by user functions, and checkpointed consistently by the system as part of the distributed snapshots.
    /// The state is only accessible by functions applied on a KeyedStream. The key is automatically supplied by the system, so the function always sees the value mapped to the key of the current element. That way, the system can handle stream and state partitioning consistently together.
    /// </summary>
    /// <typeparam name="TValue">Type of the value in the state.</typeparam>
    public interface IValueState<TValue> : IState
    {
        /// <summary>
        /// Returns and updates the current value corresponding to the current input for the state.
        /// When the state is not partitioned the returned value is the same for all inputs in a given operator instance.
        /// If state partitioning is applied, the value returned depends on the current operator input, as the operator maintains an independent state for each partition.
        /// </summary>
        /// <exception cref="System.IO.IOException">Thrown if the system cannot access the state.</exception>
        TValue Value { get; }

        /// <summary>
        /// Updates the operator state accessible by <see cref="Value"/> to the given value.
        /// The next time <see cref="Value"/> is called (for the same state partition) the returned state will represent the updated value.
        /// When a partitioned state is updated with null, the state for the current key will be removed and the default value is returned on the next access.
        /// </summary>
        /// <param name="value">The new value for the state.</param>
        /// <exception cref="System.IO.IOException">Thrown if the system cannot access the state.</exception>
        void Update(TValue value);
    }
}
