using System.Collections.Generic;
using FLink.Core.Api.Common.States;

namespace FLink.Runtime.State.Internal
{
    /// <summary>
    /// The peer to the <see cref="IListState{TValue}"/> in the internal state type hierarchy.
    /// </summary>
    /// <typeparam name="TKey">The type of key the state is associated to</typeparam>
    /// <typeparam name="TNamespace">The type of the namespace</typeparam>
    /// <typeparam name="TElement">The type of elements in the list</typeparam>
    public interface IInternalListState<TKey, TNamespace, TElement> : IInternalMergingState<TKey, TNamespace, TElement, List<TElement>, IEnumerable<TElement>>, IListState<TElement>
    {
    }
}
