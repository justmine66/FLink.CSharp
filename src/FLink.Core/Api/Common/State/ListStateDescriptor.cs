using System.Collections.Generic;

namespace FLink.Core.Api.Common.State
{
    public class ListStateDescriptor<T> : StateDescriptor<IListState<T>, IList<T>>
    {
        public ListStateDescriptor(string name, IList<T> defaultValue = default) 
            : base(name, defaultValue)
        {
        }
    }
}
