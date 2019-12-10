namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// Extension of <see cref="IAppendingState{TIn,TOut}"/> that allows merging of state.
    /// That is, two instances of <see cref="IMergingState{TIn,TOut}"/> can be combined into a single instance that contains all the information of the two merged states.
    /// </summary>
    /// <typeparam name="TInput">Type of the value that can be added to the state.</typeparam>
    /// <typeparam name="TOutput">Type of the value that can be retrieved from the state.</typeparam>
    public interface IMergingState<in TInput, out TOutput> : IAppendingState<TInput, TOutput> { }
}
