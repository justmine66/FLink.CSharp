using System.Collections.Generic;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.State
{
    public class ListStateDescriptor<T> : StateDescriptor<IListState<T>, IList<T>>
    {
        public ListStateDescriptor(string name, TypeSerializer<IList<T>> serializer, IList<T> defaultValue = default) : base(name, serializer, defaultValue)
        {
        }
    }
}
