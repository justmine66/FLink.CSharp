using System.Collections.Generic;

namespace FLink.Streaming.Api.Collector.Selector
{
    /// <summary>
    /// Interface for defining an OutputSelector.
    /// </summary>
    /// <typeparam name="TOutput">Type parameter of the split values.</typeparam>
    public interface IOutputSelector<in TOutput>
    {
        IEnumerable<string> Select(TOutput value);
    }
}
