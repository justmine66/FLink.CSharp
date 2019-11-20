using FLink.Runtime.State.MetaInfo;

namespace FLink.Runtime.State
{
    public class RegisteredKeyValueStateBackendMetaInfo<N, S> : RegisteredStateMetaInfoBase
    {
        public RegisteredKeyValueStateBackendMetaInfo(string name) : base(name)
        {
        }

        public override StateMetaInfoSnapshot Snapshot()
        {
            throw new System.NotImplementedException();
        }
    }
}
