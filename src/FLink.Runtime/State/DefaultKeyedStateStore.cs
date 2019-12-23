using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.States;
using FLink.Core.Util;

namespace FLink.Runtime.State
{
    /// <summary>
    /// Default implementation of KeyedStateStore that currently forwards state registration to a <see cref="IRuntimeContext"/>.
    /// </summary>
    public class DefaultKeyedStateStore<TKey> : IKeyedStateStore
    {
        protected IKeyedStateBackend<TKey> KeyedStateBackend;
        protected ExecutionConfig ExecutionConfig;

        public DefaultKeyedStateStore(IKeyedStateBackend<TKey> keyedStateBackend, ExecutionConfig executionConfig)
        {
            KeyedStateBackend = Preconditions.CheckNotNull(keyedStateBackend);
            ExecutionConfig = Preconditions.CheckNotNull(executionConfig);
        }

        public virtual IValueState<TValue> GetState<TValue>(ValueStateDescriptor<TValue> stateProperties)
        {
            throw new NotImplementedException();
        }

        public virtual IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateProperties)
        {
            throw new NotImplementedException();
        }

        public virtual IReducingState<TValue> GetReducingState<TValue>(ReducingStateDescriptor<TValue> stateProperties)
        {
            throw new NotImplementedException();
        }

        public virtual IAggregatingState<TInput, TOutput> GetAggregatingState<TInput, TAccumulator, TOutput>(AggregatingStateDescriptor<TInput, TAccumulator, TOutput> stateProperties)
        {
            throw new NotImplementedException();
        }

        public virtual IMapState<TK, TValue> GetMapState<TK, TValue>(MapStateDescriptor<TK, TValue> stateProperties)
        {
            throw new NotImplementedException();
        }

        protected virtual TState GetPartitionedState<TState, TValue>(StateDescriptor<TState, TValue> stateDescriptor) 
            where TState : IState
        {
            return KeyedStateBackend.GetPartitionedState(VoidNamespace.Instance, VoidNamespaceSerializer.Instance, stateDescriptor);
        }
    }
}
