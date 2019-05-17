using System.Collections.Generic;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// <see cref="IState"/> interface for partitioned list state in Operations.
    /// The state is accessed and modified by user functions, and checkpointed consistently by the system as part of the distributed snapshots.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListState<T> : IMergingState<T, IEnumerable<T>>
    {
        void Update(List<T> values);

        void AddAll(List<T> values);
    }
}
