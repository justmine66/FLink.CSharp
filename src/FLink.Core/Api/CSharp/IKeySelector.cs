using FLink.Core.Api.Common.Functions;

namespace FLink.Core.Api.CSharp
{
    public interface IKeySelector<in TInput, out TKey> : IFunction
    {
        TKey GetKey(TInput value);
    }
}
