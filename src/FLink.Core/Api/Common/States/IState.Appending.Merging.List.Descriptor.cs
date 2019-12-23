using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.States
{
    /// <summary>
    /// A <see cref="ListStateDescriptor{T}"/> for <see cref="IListState{TValue}"/>. This can be used to create state where the type is a list that can be appended and iterated over.
    /// Using <see cref="IListState{TValue}"/> is typically more efficient than manually maintaining a list in a <see cref="IValueState{TValue}"/>, because the backing implementation can support efficient appends, rather than replacing the full list on write.
    /// </summary>
    /// <typeparam name="TValue">The type of the values that can be added to the list state.</typeparam>
    public class ListStateDescriptor<TValue> : StateDescriptor<IListState<TValue>, IList<TValue>>
    {
        public ListStateDescriptor(string name, IList<TValue> defaultValue = default)
            : base(name, typeof(TValue), defaultValue) { }

        public ListStateDescriptor(string name, System.Type type, IList<TValue> defaultValue = default)
            : base(name, type, defaultValue) { }

        public ListStateDescriptor(string name, TypeInformation<IList<TValue>> typeInfo, IList<TValue> defaultValue = default)
            : base(name, typeInfo, defaultValue) { }

        public ListStateDescriptor(string name, TypeSerializer<IList<TValue>> serializer, IList<TValue> defaultValue = default)
            : base(name, serializer, defaultValue) { }

        public override StateType Type => StateType.List;
    }
}
