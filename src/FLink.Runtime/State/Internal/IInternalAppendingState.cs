using FLink.Core.Api.Common.States;

namespace FLink.Runtime.State.Internal
{
    /// <summary>
    /// The peer to the <see cref="IAppendingState{TIn,TOut}"/> in the internal state type hierarchy.
    /// See <see cref="IInternalKvState{TKey,TNamespace,TValue}"/> for a description of the internal state hierarchy.
    /// </summary>
    /// <typeparam name="TKey">The type of key the state is associated to</typeparam>
    /// <typeparam name="TNamespace">The type of the namespace</typeparam>
    /// <typeparam name="TInput">The type of elements added to the state</typeparam>
    /// <typeparam name="TElement">The type of elements in the state</typeparam>
    /// <typeparam name="TOutput">The type of the resulting element in the state</typeparam>
    public interface IInternalAppendingState<TKey, TNamespace, in TInput, TElement, out TOutput> : IInternalKvState<TKey, TNamespace, TElement>, IAppendingState<TInput, TOutput>
    {
        /// <summary>
        /// Gets and sets internally stored value.
        /// </summary>
        TElement Internal { get; set; }
    }
}
