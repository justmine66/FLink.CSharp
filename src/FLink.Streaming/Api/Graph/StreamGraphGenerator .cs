using FLink.Runtime.State;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// A generator that generates a <see cref="StreamGraph"/> from a graph of <see cref="tr"/>s.
    /// </summary>
    public class StreamGraphGenerator
    {
        public static readonly int DefaultLowerBoundMaxParallelism = KeyGroupRangeAssignment.DefaultLowerBoundMaxParallelism;
    }
}
