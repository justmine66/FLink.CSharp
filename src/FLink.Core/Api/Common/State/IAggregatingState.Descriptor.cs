using FLink.Core.Api.Common.Accumulators;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// A StateDescriptor for <see cref="IAggregatingState{TInput,TOutput}"/>.
    /// The type internally stored in the state is the type of the <see cref="IAccumulator{TValue,TResult}"/> of the <see cref="IAggregateFunction{TInput,TAccumulator,TOutput}"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the values that are added to the state.</typeparam>
    /// <typeparam name="TAccumulator">The type of the accumulator (intermediate aggregation state).</typeparam>
    /// <typeparam name="TOutput">The type of the values that are returned from the state.</typeparam>
    public class AggregatingStateDescriptor<TInput, TAccumulator, TOutput> : StateDescriptor<IAggregatingState<TInput, TOutput>, TAccumulator>
    {
        public AggregatingStateDescriptor(string name, TypeSerializer<TAccumulator> serializer, TAccumulator defaultValue = default) 
            : base(name, serializer, defaultValue)
        {
        }
    }
}
