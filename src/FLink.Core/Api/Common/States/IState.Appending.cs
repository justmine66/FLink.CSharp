namespace FLink.Core.Api.Common.States
{
    /// <summary>
    /// Base interface for partitioned state that supports adding elements and inspecting the current state.
    /// Elements can either be kept in a buffer (list-like) or aggregated into one value.
    /// </summary>
    /// <typeparam name="TInput">Type of the value that can be added to the state.</typeparam>
    /// <typeparam name="TOutput">Type of the value that can be retrieved from the state.</typeparam>
    public interface IAppendingState<in TInput, out TOutput> : IState
    {
        /// <summary>
        /// Returns the current value for the state. 
        /// </summary>
        /// <returns></returns>
        TOutput Get();

        /// <summary>
        /// Updates the operator state accessible by <see cref="Get"/> by adding the given value to the list of values.
        /// </summary>
        /// <param name="value"></param>
        void Add(TInput value);
    }
}
