using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Operators;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Api.CSharp.TypeUtils;

namespace FLink.Streaming.Util.Keys
{
    /// <summary>
    /// Utility class that contains helper methods to manipulating <see cref="IKeySelector{TObject,TKey}"/> for streaming.
    /// </summary>
    public class KeySelectorUtil
    {
        public static ArrayKeySelector<TX, TKey> GetSelectorForArray<TX, TKey>(int[] positions, TypeInformation<TX> typeInfo)
        {
            return null;
        }

        public static IKeySelector<TObject, TKey> GetSelectorForKeys<TObject, TKey>(Keys<TObject> keys, TypeInformation<TObject> typeInfo, ExecutionConfig executionConfig)
        {
            return null;
        }
    }

    public sealed class ArrayKeySelector<TIn, TKey> : IKeySelector<TIn, TKey>, IResultTypeQueryable<TKey>
    {
        public TypeInformation<TKey> ProducedType => throw new NotImplementedException();

        public TKey GetKey(TIn value)
        {
            throw new NotImplementedException();
        }
    }
}
