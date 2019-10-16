using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="StateDescriptor{TState,T}"/> for <see cref="IReducingState{T}"/>. This can be used to create partitioned reducing state.
    /// </summary>
    /// <typeparam name="T">The type of the values that can be added to the list state.</typeparam>
    public class ReducingStateDescriptor<T> : StateDescriptor<IReducingState<T>, T>
    {
        public ReducingStateDescriptor(string name, TypeSerializer<T> serializer, T defaultValue = default) 
            : base(name, serializer, defaultValue)
        {
        }
    }
}
