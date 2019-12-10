using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="StateDescriptor{TState,T}"/> for <see cref="IReducingState{T}"/>. This can be used to create partitioned reducing state.
    /// </summary>
    /// <typeparam name="TValue">The type of the values that can be added to the list state.</typeparam>
    public class ReducingStateDescriptor<TValue> : StateDescriptor<IReducingState<TValue>, TValue>
    {
        public ReducingStateDescriptor(string name,
            IReduceFunction<TValue> reduceFunction,
            TValue defaultValue = default)
            : base(name, typeof(TValue), defaultValue)
        {
        }

        public ReducingStateDescriptor(string name,
            IReduceFunction<TValue> reduceFunction,
            System.Type type, 
            TValue defaultValue = default)
            : base(name, type, defaultValue)
        {
        }

        public ReducingStateDescriptor(string name, 
            IReduceFunction<TValue> reduceFunction, 
            TypeSerializer<TValue> typeSerializer,
            TValue defaultValue = default)
            : base(name, typeSerializer, defaultValue)
        {
            ReduceFunction = reduceFunction;
        }

        public ReducingStateDescriptor(string name,
            IReduceFunction<TValue> reduceFunction,
            TypeInformation<TValue> typeInfo,
            TValue defaultValue = default)
            : base(name, typeInfo, defaultValue)
        {
            ReduceFunction = reduceFunction;
        }

        /// <summary>
        /// Gets the reduce function to be used for the reducing state.
        /// </summary>
        public IReduceFunction<TValue> ReduceFunction { get; }

        public override StateType Type => StateType.Reducing;
    }
}
