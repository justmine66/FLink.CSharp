using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="ValueStateDescriptor{T}"/> for <see cref="IValueState{TValue}"/>.
    /// This can be used to create partitioned value state.
    /// </summary>
    /// <typeparam name="TValue">The type of the values that the value state can hold.</typeparam>
    public class ValueStateDescriptor<TValue> : StateDescriptor<IValueState<TValue>, TValue>
    {
        public ValueStateDescriptor(string name, TValue defaultValue = default)
            : base(name, typeof(TValue), defaultValue)
        { }

        public ValueStateDescriptor(string name, System.Type type, TValue defaultValue = default)
            : base(name, type, defaultValue)
        { }

        public ValueStateDescriptor(string name, TypeInformation<TValue> typeInfo, TValue defaultValue = default) :
            base(name, typeInfo, defaultValue)
        { }

        public ValueStateDescriptor(string name, TypeSerializer<TValue> serializer, TValue defaultValue = default)
            : base(name, serializer, defaultValue)
        { }

        public override StateType Type => StateType.Value;
    }
}
