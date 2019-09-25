namespace FLink.Streaming.Api.Operators
{
    public class SimpleOperatorFactory<TOut> : IStreamOperatorFactory<TOut>
    {
        public SimpleOperatorFactory(IStreamOperator<TOut> @operator)
        {

        }

        public static SimpleOperatorFactory<TOut> Of(IStreamOperator<TOut> @operator)
        {
            return default;
        }
    }
}
