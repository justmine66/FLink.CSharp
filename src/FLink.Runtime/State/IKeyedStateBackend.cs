using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Runtime.State
{
    /// <summary>
    /// A keyed state backend provides methods for managing keyed state.
    /// </summary>
    /// <typeparam name="TKey">The key by which state is keyed.</typeparam>
    public interface IKeyedStateBackend<TKey> : IKeyedStateFactory, IPriorityQueueSetFactory, IDisposable
    {
        /// <summary>
        /// Gets or sets the current key that is used for partitioned state.
        /// </summary>
        TKey CurrentKey { get; set; }

        /// <summary>
        /// Serializer of the key.
        /// </summary>
        /// <returns></returns>
        TypeSerializer<TKey> GetKeySerializer();

        /// <summary>
        /// Applies the provided <see cref="IKeyedStateFunction{TKey,TState}"/> to the state with the provided <see cref="StateDescriptor{TState,T}"/> of all the currently active keys.
        /// </summary>
        /// <typeparam name="TNamespace">The type of the namespace.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="namespace">the namespace of the state.</param>
        /// <param name="namespaceSerializer">the serializer for the namespace.</param>
        /// <param name="stateDescriptor">the descriptor of the state to which the function is going to be applied.</param>
        /// <param name="function">the function to be applied to the keyed state.</param>
        void ApplyToAllKeys<TNamespace, TState, T>(
            TNamespace @namespace,
            TypeSerializer<TNamespace> namespaceSerializer,
            StateDescriptor<TState, T> stateDescriptor,
            IKeyedStateFunction<TKey, TState> function) where TState : IState;

        /// <summary>
        /// A stream of all keys for the given state and namespace. Modifications to the state during iterating over it keys are not supported.
        /// </summary>
        /// <typeparam name="TNamespace"></typeparam>
        /// <param name="state">State variable for which existing keys will be returned.</param>
        /// <param name="namespace">Namespace for which existing keys will be returned.</param>
        /// <returns></returns>
        IEnumerable<TKey> GetKeys<TNamespace>(string state, TNamespace @namespace);

        /// <summary>
        /// Creates or retrieves a keyed state backed by this state backend.
        /// </summary>
        /// <typeparam name="TNamespace">The type of the namespace.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="namespaceSerializer">The serializer used for the namespace type of the state</param>
        /// <param name="stateDescriptor">The identifier for the state. This contains name and can create a default state value.</param>
        /// <returns>A new key/value state backed by this backend.</returns>
        /// <exception cref="Exception">Exceptions may occur during initialization of the state and should be forwarded.</exception>
        TState GetOrCreateKeyedState<TNamespace, TState, T>(
            TypeSerializer<TNamespace> namespaceSerializer,
            StateDescriptor<TState, T> stateDescriptor) where TState : IState;

        /// <summary>
        /// Creates or retrieves a partitioned state backed by this state backend.
        /// </summary>
        /// <typeparam name="TNamespace">The type of the namespace.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="namespace"></param>
        /// <param name="namespaceSerializer"></param>
        /// <param name="stateDescriptor">The identifier for the state. This contains name and can create a default state value.</param>
        /// <returns>A new key/value state backed by this backend.  </returns>
        /// <exception cref="Exception">Exceptions may occur during initialization of the state and should be forwarded.</exception>
        TState GetPartitionedState<TNamespace, TState, TValue>(
            TNamespace @namespace,
            TypeSerializer<TNamespace> namespaceSerializer,
            StateDescriptor<TState, TValue> stateDescriptor) where TState : IState;

        /// <summary>
        /// State backend will call <see cref="IKeySelectionListener{TKey}"/> when key context is switched if supported. 
        /// </summary>
        /// <param name="listener"></param>
        void RegisterKeySelectionListener(IKeySelectionListener<TKey> listener);

        /// <summary>
        /// Stop calling listener registered in <see cref="RegisterKeySelectionListener"/>.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns>true iff listener was registered before.</returns>
        bool DeRegisterKeySelectionListener(IKeySelectionListener<TKey> listener);
    }

    public interface IKeySelectionListener<in TKey>
    {
        /// <summary>
        /// Callback when key context is switched.
        /// </summary>
        /// <param name="newKey"></param>
        void KeySelected(TKey newKey);
    }
}
