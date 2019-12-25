using System.Collections.Generic;
using FLink.Core.Api.Common.States;

namespace FLink.Runtime.State
{
    /// <summary>
    /// Implementation of operator list state.
    /// </summary>
    /// <typeparam name="TState">the type of an operator state partition.</typeparam>
    public class PartitionableListState<TState> : IListState<TState>
    {
        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TState> Get()
        {
            throw new System.NotImplementedException();
        }

        public void Add(TState value)
        {
            throw new System.NotImplementedException();
        }

        public void Update(List<TState> values)
        {
            throw new System.NotImplementedException();
        }

        public void AddAll(List<TState> values)
        {
            throw new System.NotImplementedException();
        }
    }
}
