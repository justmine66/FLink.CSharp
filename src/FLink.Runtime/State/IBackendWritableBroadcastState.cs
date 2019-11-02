using FLink.Core.Api.Common.State;
using FLink.Core.FS;

namespace FLink.Runtime.State
{
    public interface IBackendWritableBroadcastState<K, V> : IBroadcastState<K, V>
    {
        IBackendWritableBroadcastState<K, V> DeepCopy();
        long Write(FSDataOutputStream output);
        void SetStateMetaInfo(RegisteredBroadcastStateBackendMetaInfo<K, V> stateMetaInfo);
        RegisteredBroadcastStateBackendMetaInfo<K, V> GetStateMetaInfo();
    }
}
