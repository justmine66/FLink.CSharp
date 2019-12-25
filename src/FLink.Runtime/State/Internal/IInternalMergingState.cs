using System.Collections.Generic;
using FLink.Core.Api.Common.States;
using System;

namespace FLink.Runtime.State.Internal
{
    /// <summary>
    /// The peer to the <see cref="IMergingState{TInput,TOutput}"/> in the internal state type hierarchy.
    /// </summary>
    /// <typeparam name="TKey">The type of key the state is associated to</typeparam>
    /// <typeparam name="TNamespace">The type of the namespace</typeparam>
    /// <typeparam name="TInput">The type of elements added to the state</typeparam>
    /// <typeparam name="TElement">The type of elements in the state</typeparam>
    /// <typeparam name="TOutput">The type of elements</typeparam>
    public interface IInternalMergingState<TKey, TNamespace, in TInput, TElement, out TOutput> : IInternalAppendingState<TKey, TNamespace, TInput, TElement, TOutput>, IMergingState<TInput, TOutput>
    {
        /// <summary>
        /// Merges the state of the current key for the given source namespaces into the state of the target namespace.
        /// </summary>
        /// <param name="target">The target namespace where the merged state should be stored.</param>
        /// <param name="sources">The source namespaces whose state should be merged.</param>
        /// <exception cref="Exception">The method may forward exception thrown internally (by I/O or functions).</exception>
        void MergeNamespaces(TNamespace target, IList<TNamespace> sources);
    }
}
