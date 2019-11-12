using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Checkpoint;

namespace FLink.Streaming.Api.Operators
{
    public abstract class AbstractUdfStreamOperator<TOut, TFunction> : AbstractStreamOperator<TOut>, IOutputTypeConfigurable<TOut>
        where TFunction : IFunction
    {
        private readonly bool _functionsClosed = false;

        /// <summary>
        /// The user function. 
        /// </summary>
        public TFunction UserFunction;

        protected AbstractUdfStreamOperator(TFunction userFunction) => UserFunction = CheckUdfCheckpointing(userFunction);

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

        private TFunction CheckUdfCheckpointing(TFunction function)
        {
            if (function is ICheckpointedFunction && function is IListCheckpointed<TOut>)
                throw new IllegalStateException("User functions are not allowed to implement " +
                                                "ICheckpointedFunction AND IListCheckpointed.");

            return function;
        }
    }
}
