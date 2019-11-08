using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="ValueStateDescriptor{T}"/> for <see cref="IValueState{TValue}"/>.
    /// This can be used to create partitioned value state.
    /// </summary>
    /// <typeparam name="T">The type of the values that the value state can hold.</typeparam>
    public class ValueStateDescriptor<T> : StateDescriptor<IValueState<T>, T>
    {
        public ValueStateDescriptor(string name, T defaultValue = default) 
            : base(name, defaultValue)
        { }

        public ValueStateDescriptor(string name, TypeSerializer<T> serializer, T defaultValue = default)
            : base(name, serializer, defaultValue)
        {
        }
    }
}
