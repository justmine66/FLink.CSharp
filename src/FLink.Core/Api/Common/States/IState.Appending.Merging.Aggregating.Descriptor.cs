using FLink.Core.Api.Common.Accumulators;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.States
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
        public AggregatingStateDescriptor(
            string name,
            IAggregateFunction<TInput, TAccumulator, TOutput> aggFunction,
            TypeInformation<TAccumulator> stateType,
            TAccumulator defaultValue = default)
            : base(name, stateType, defaultValue)
        {
            AggregateFunction = aggFunction;
        }

        public AggregatingStateDescriptor(
            string name,
            IAggregateFunction<TInput, TAccumulator, TOutput> aggFunction,
            TypeSerializer<TAccumulator> stateType,
            TAccumulator defaultValue = default)
            : base(name, stateType, defaultValue)
        {
            AggregateFunction = aggFunction;
        }

        /// <summary>
        /// Gets the aggregate function to be used for the state.
        /// </summary>
        public IAggregateFunction<TInput, TAccumulator, TOutput> AggregateFunction { get; }

        public override StateType Type => StateType.Aggregating;
    }
}
