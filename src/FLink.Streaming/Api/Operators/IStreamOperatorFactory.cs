namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A factory to create <see cref="IStreamOperator{TOutput}"/>
    /// </summary>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public interface IStreamOperatorFactory<TOutput>
    {
        /// <summary>
        /// Gets and sets the chaining strategy for operator factory.
        /// </summary>
        ChainingStrategy ChainingStrategy { get; set; }

        /// <summary>
        /// Is this factory for <see cref="StreamSource{TOut,TSrc}"/>.
        /// </summary>
        bool IsStreamSource { get; }
    }
}
