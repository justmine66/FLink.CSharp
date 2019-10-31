using System;

namespace FLink.Runtime.State
{
    public class SnapshotResult<T> : IStateObject where T : IStateObject
    {
        public void DiscardState()
        {
            throw new NotImplementedException();
        }

        public void GetStateSize()
        {
            throw new NotImplementedException();
        }
    }
}
