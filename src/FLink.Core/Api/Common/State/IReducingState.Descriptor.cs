using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="StateDescriptor{TState,T}"/> for <see cref="IReducingState{T}"/>. This can be used to create partitioned reducing state.
    /// </summary>
    /// <typeparam name="TValue">The type of the values that can be added to the list state.</typeparam>
    public class ReducingStateDescriptor<TValue> : StateDescriptor<IReducingState<TValue>, TValue>
    {
        public ReducingStateDescriptor(string name, TypeSerializer<TValue> serializer, TValue defaultValue = default)
            : base(name, serializer, defaultValue)
        {
        }

        public ReducingStateDescriptor(string name, IReduceFunction<TValue> reduceFunction, TypeSerializer<TValue> typeSerializer)
            : base(name, typeSerializer, default)
        {

        }
    }
}
