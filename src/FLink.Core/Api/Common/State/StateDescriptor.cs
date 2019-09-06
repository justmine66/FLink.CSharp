namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// Base class for state descriptors.
    /// A <see cref="StateDescriptor{TState,T}"/> is used for creating partitioned <see cref="IState"/> in stateful operations.
    /// </summary>
    public abstract class StateDescriptor<TState, T> where TState : IState
    {
        /// <summary>
        /// An enumeration of the types of supported states.
        /// Used to identify the state type when writing and restoring checkpoints and savepoints.
        /// </summary>
        public enum Type
        {
            Value,
            List,
            Reducing,
            Aggregating,
            Map
        }

        /// <summary>
        /// Name that uniquely identifies state created from this StateDescriptor.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The default value returned by the state when no other value is bound to a key.
        /// </summary>
        public T DefaultValue { get; protected set; }

        /// <summary>
        /// Returns the queryable state name.
        /// </summary>
        public string QueryableStateName { get; protected set; }

        public bool IsQueryable()
        {
            return !string.IsNullOrEmpty(QueryableStateName);
        }

        protected StateDescriptor(string name, T defaultValue = default)
        {
            Name = name;
        }
    }
}
