namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="IState"/> interface for partitioned single-value state. The value can be retrieved or updated.
    /// </summary>
    /// <typeparam name="TValue">Type of the value in the state.</typeparam>
    public interface IValueState<TValue>
    {
        /// <summary>
        /// Returns and updates the current value for the state.
        /// </summary>
        TValue Value { get; set; }
    }
}
