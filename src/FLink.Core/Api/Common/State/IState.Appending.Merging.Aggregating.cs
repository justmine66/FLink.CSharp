using FLink.Core.Api.Common.Functions;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="IState"/> interface for aggregating state, based on an <see cref="IAggregateFunction{TIn,TAcc,TOut}"/>. Elements that are added to this type of state will be eagerly pre-aggregated using a given {@code AggregateFunction}.
    /// </summary>
    /// <typeparam name="TInput">Type of the value added to the state.</typeparam>
    /// <typeparam name="TOutput">Type of the value extracted from the state.</typeparam>
    public interface IAggregatingState<in TInput, out TOutput> : IMergingState<TInput, TOutput> { }
}
