using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Runtime.State.Internal
{
    /// <summary>
    /// The <see cref="IInternalKvState{TKey,TNamespace,TValue}"/> is the root of the internal state type hierarchy, similar to the <see cref="IState"/> being the root of the public API state hierarchy.
    /// </summary>
    /// <typeparam name="TKey">The type of key the state is associated to</typeparam>
    /// <typeparam name="TNamespace">The type of the namespace</typeparam>
    /// <typeparam name="TValue">The type of values kept internally in state</typeparam>
    public interface IInternalKvState<TKey, TNamespace, TValue> : IState
    {
        /// <summary>
        /// Returns the <see cref="TypeSerializer{T}"/> for the type of key this state is associated to.
        /// </summary>
        /// <returns></returns>
        TypeSerializer<TKey> GetKeySerializer();

        /// <summary>
        /// Returns the <see cref="TypeSerializer{T}"/> for the type of namespace this state is associated to.
        /// </summary>
        /// <returns></returns>
        TypeSerializer<TNamespace> GetNamespaceSerializer();

        /// <summary>
        /// Returns the <see cref="TypeSerializer{T}"/> for the type of value this state holds.
        /// </summary>
        /// <returns></returns>
        TypeSerializer<TValue> GetValueSerializer();

        /// <summary>
        /// Sets the current namespace, which will be used when using the state access methods.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        void SetCurrentNamespace(TNamespace @namespace);

        /// <summary>
        /// Returns the serialized value for the given key and namespace.
        /// </summary>
        /// <param name="serializedKeyAndNamespace">Serialized key and namespace</param>
        /// <param name="safeKeySerializer">A key serializer which is safe to be used even in multi-threaded context</param>
        /// <param name="safeNamespaceSerializer">A namespace serializer which is safe to be used even in multi-threaded context</param>
        /// <param name="safeValueSerializer">A value serializer which is safe to be used even in multi-threaded context</param>
        /// <returns>Serialized value or <code>null</code> if no value is associated with the key and namespace.</returns>
        /// <exception cref="System.Exception">Exceptions during serialization are forwarded</exception>
        byte[] GetSerializedValue(byte[] serializedKeyAndNamespace, TypeSerializer<TKey> safeKeySerializer,
            TypeSerializer<TNamespace> safeNamespaceSerializer, TypeSerializer<TValue> safeValueSerializer);
    }
}
