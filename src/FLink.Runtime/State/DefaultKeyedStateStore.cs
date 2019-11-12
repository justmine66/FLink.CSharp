using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.State;

namespace FLink.Runtime.State
{
    /// <summary>
    /// Default implementation of KeyedStateStore that currently forwards state registration to a <see cref="IRuntimeContext"/>.
    /// </summary>
    public class DefaultKeyedStateStore : IKeyedStateStore
    {
        public IValueState<TValue> GetState<TValue>(ValueStateDescriptor<TValue> stateProperties)
        {
            throw new NotImplementedException();
        }

        public IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateProperties)
        {
            throw new NotImplementedException();
        }

        public IReducingState<TValue> GetReducingState<TValue>(ReducingStateDescriptor<TValue> stateProperties)
        {
            throw new NotImplementedException();
        }

        public IAggregatingState<TInput, TOutput> GetAggregatingState<TInput, TAccumulator, TOutput>(AggregatingStateDescriptor<TInput, TAccumulator, TOutput> stateProperties)
        {
            throw new NotImplementedException();
        }

        public IMapState<TKey, TValue> GetMapState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateProperties)
        {
            throw new NotImplementedException();
        }
    }
}
