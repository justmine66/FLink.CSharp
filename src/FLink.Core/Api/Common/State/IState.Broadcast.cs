using System.Collections.Generic;

namespace FLink.Core.Api.Common.State
{
    public interface IBroadcastState<TKey, TValue> : IReadOnlyBroadcastState<TKey, TValue>, IEnumerable<TValue>
    {
        void Put(TKey key, TValue value);

        void PutAll(Dictionary<TKey, TValue> map);

        void Remove(TKey key);

        IEnumerable<KeyValuePair<TKey, TValue>> Entries { get; }
    }
}
