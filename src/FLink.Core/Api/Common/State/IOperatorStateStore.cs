namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// This interface contains methods for registering operator state with a managed store.
    /// </summary>
    public interface IOperatorStateStore
    {
        /// <summary>
        /// Creates (or restores) a list state. Each state is registered under a unique name.
        /// The provided serializer is used to de/serialize the state in case of checkpointing(snapshot/restore).
        /// <remarks>
        /// Note the semantic differences between an operator list state and a keyed list state.
        /// </remarks>
        /// </summary>
        /// <typeparam name="T">The generic type of the state</typeparam>
        /// <param name="stateDescriptor">The descriptor for this state, providing a name and serializer.</param>
        /// <returns>A list for all state partitions.</returns>
        IListState<T> GetListState<T>(ListStateDescriptor<T> stateDescriptor);
    }
}
