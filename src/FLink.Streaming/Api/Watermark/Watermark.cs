using FLink.Streaming.Runtime.StreamRecord;

namespace FLink.Streaming.Api.Watermark
{
    /// <summary>
    /// A Watermark tells operators that no elements with a timestamp older or equal to the watermark timestamp should arrive at the operator. Watermarks are emitted at the sources and propagate through the operators of the topology. Operators must themselves emit watermarks to downstream operators using
    /// </summary>
    public class Watermark : StreamElement
    {

    }
}
