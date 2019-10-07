using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Functions;

namespace FLink.Streaming.Api.Operators
{
    public class AbstractUdfStreamOperator<TOut, TFunction> : AbstractStreamOperator<TOut>, IOutputTypeConfigurable<TOut>
        where TFunction : IFunction
    {
        public AbstractUdfStreamOperator(TFunction userFunction)
        {
             
        }

        public void SetOutputType(Type outTypeInfo, ExecutionConfig executionConfig)
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }
    }
}
