namespace FLink.Streaming.Api.Operators
{
    using static FLink.Core.Util.Preconditions;

    public class SimpleOperatorFactory<TTOut> : IStreamOperatorFactory<TTOut>
    {
        public SimpleOperatorFactory(IStreamOperator<TTOut> @operator)
        {
            
        }
    }
}
