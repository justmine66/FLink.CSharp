namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A factory to create <see cref="IStreamOperator{TOutput}"/>
    /// </summary>
    /// <typeparam name="T">The output type of the operator</typeparam>
    public interface IStreamOperatorFactory<T>
    {
        /// <summary>
        /// Set and set the chaining strategy for operator factory.
        /// </summary>
        ChainingStrategy ChainingStrategy { get; set; }
    }
}
