using System;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Transformations
{
    public class OneInputTransformation<TIn, TOut> : PhysicalTransformation<TOut>
    {
        public OneInputTransformation(string name, TypeInformation<TOut> outputType, int parallelism) : base(name, outputType, parallelism)
        {
        }

        public override void SetChainingStrategy(ChainingStrategy strategy)
        {
            throw new NotImplementedException();
        }
    }
}
