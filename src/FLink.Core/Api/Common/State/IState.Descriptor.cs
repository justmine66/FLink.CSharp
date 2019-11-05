using System;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.State
{
    using static Preconditions;

    /// <summary>
    /// Base class for state descriptors.
    /// A <see cref="StateDescriptor{TState,T}"/> is used for creating partitioned <see cref="IState"/> in stateful operations.
    /// </summary>
    [Serializable]
    public abstract class StateDescriptor<TState, TValue> where TState : IState
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
            Folding,
            Aggregating,
            Map
        }

        /// <summary>
        /// Name that uniquely identifies state created from this StateDescriptor.
        /// </summary>
        public string Name;

        /// <summary>
        /// The default value returned by the state when no other value is bound to a key.
        /// </summary>
        public TValue DefaultValue;

        /// <summary>
        /// The serializer for the type. May be eagerly initialized in the constructor or lazily.
        /// </summary>
        public TypeSerializer<TValue> Serializer;

        /// <summary>
        /// Returns the queryable state name.
        /// </summary>
        public string QueryableStateName;

        /// <summary>
        /// Returns whether the state created from this descriptor is queryable.
        /// </summary>
        public bool IsQueryable => !string.IsNullOrEmpty(QueryableStateName);

        /// <summary>
        /// Create a new <see cref="StateDescriptor{TState,T}"/> with the given name and the given type serializer.
        /// </summary>
        /// <param name="name">The name of the <see cref="StateDescriptor{TState,T}"/>.</param>
        /// <param name="serializer">The type serializer for the values in the state.</param>
        /// <param name="defaultValue">The default value that will be set when requesting state without setting a value before.</param>
        protected StateDescriptor(string name, TypeSerializer<TValue> serializer, TValue defaultValue = default)
        {
            Name = CheckNotNull(name);
            Serializer = CheckNotNull(serializer, "serializer must not be null");
            DefaultValue = defaultValue;
        }
    }
}
