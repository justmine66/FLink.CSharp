using FLink.Core.Api.Common.States;

namespace FLink.Runtime.State
{
    /// <summary>
    /// A function to be applied to all keyed states.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TState">The type of state.</typeparam>
    public interface IKeyedStateFunction<in TKey, in TState> where TState : IState
    {
        /// <summary>
        /// The actual method to be applied on each of the states.
        /// </summary>
        /// <param name="key">the key whose state is being processed.</param>
        /// <param name="state">the state associated with the aforementioned key.</param>
        void Process(TKey key, TState state);
    }
}
