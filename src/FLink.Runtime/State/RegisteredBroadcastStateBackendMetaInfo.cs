using FLink.Runtime.State.MetaInfo;

namespace FLink.Runtime.State
{
    public class RegisteredBroadcastStateBackendMetaInfo<K, V> : RegisteredStateMetaInfoBase
    {
        public RegisteredBroadcastStateBackendMetaInfo(string name) : base(name)
        {
        }

        public override StateMetaInfoSnapshot Snapshot()
        {
            throw new System.NotImplementedException();
        }
    }
}
