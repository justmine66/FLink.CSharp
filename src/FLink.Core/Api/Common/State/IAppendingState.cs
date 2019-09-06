namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// Base interface for partitioned state that supports adding elements and inspecting the current state.
    /// Elements can either be kept in a buffer (list-like) or aggregated into one value.
    /// </summary>
    /// <typeparam name="TIn">Type of the value that can be added to the state.</typeparam>
    /// <typeparam name="TOut">Type of the value that can be retrieved from the state.</typeparam>
    public interface IAppendingState<in TIn, out TOut> : IState
    {
        /// <summary>
        /// Returns the current value for the state. 
        /// </summary>
        /// <returns></returns>
        TOut Get();

        /// <summary>
        /// Updates the operator state accessible by <see cref="Get"/> by adding the given value to the list of values.
        /// </summary>
        /// <param name="value"></param>
        void Add(TIn value);
    }
}
