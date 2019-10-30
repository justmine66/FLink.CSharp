using FLink.Streaming.Api.DataStreams;

namespace FLink.Streaming.Api.Functions.Co
{
    /// <summary>
    /// A function to be applied to a <see cref="BroadcastConnectedStream{TInput1,TInput2}"/>.
    /// </summary>
    /// <typeparam name="TKey">The key type of the input keyed stream.</typeparam>
    /// <typeparam name="TInput1">The input type of the non-broadcast side.</typeparam>
    /// <typeparam name="TInput2">The input type of the broadcast side.</typeparam>
    /// <typeparam name="TOutput">The output type of the operator.</typeparam>
    public abstract class KeyedBroadcastProcessFunction<TKey, TInput1, TInput2, TOutput> : BaseBroadcastProcessFunction
    {

    }
}
