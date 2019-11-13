using FLink.Core.Util;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Simple factory which just wrap existed <see cref="IStreamOperator{TOutput}"/>.
    /// </summary>
    /// <typeparam name="TOut">The output type of the operator</typeparam>
    public class SimpleOperatorFactory<TOut> : IStreamOperatorFactory<TOut>
    {
        public IStreamOperator<TOut> Operator { get; }

        public SimpleOperatorFactory(IStreamOperator<TOut> @operator) => Operator = Preconditions.CheckNotNull(@operator);

        public ChainingStrategy ChainingStrategy { get; set; }

        public bool IsStreamSource => false;

        /// <summary>
        /// Create a SimpleOperatorFactory from existed StreamOperator.
        /// </summary>
        /// <param name="operator"></param>
        /// <returns></returns>
        public static SimpleOperatorFactory<TOut> Of(IStreamOperator<TOut> @operator)
        {
            return default;
        }
    }
}
