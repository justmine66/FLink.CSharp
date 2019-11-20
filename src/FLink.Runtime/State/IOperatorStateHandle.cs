namespace FLink.Runtime.State
{
    /// <summary>
    /// Interface of a state handle for operator state.
    /// </summary>
    public interface IOperatorStateHandle : IStreamStateHandle
    {

    }

    /// <summary>
    /// The modes that determine how an <see cref="IOperatorStateHandle"/>} is assigned to tasks during restore.
    /// </summary>
    public enum OperatorStateHandleMode
    {
        /// <summary>
        /// The operator state partitions in the state handle are split and distributed to one task each.
        /// </summary>
        SplitDistribute,
        /// <summary>
        /// The operator state partitions are UNION-ed upon restoring and sent to all tasks.
        /// </summary>
        Union,
        /// <summary>
        /// The operator states are identical, as the state is produced from a broadcast stream.
        /// </summary>
        Broadcast
    }
}
