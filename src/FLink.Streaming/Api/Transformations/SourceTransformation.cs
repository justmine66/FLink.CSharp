using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This represents a Source. This does not actually transform anything since it has no inputs but
    /// it is the root <see cref="Transformation{T}"/> of any topology.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SourceTransformation<T> : PhysicalTransformation<T>
    {
        public SourceTransformation(
            string name,
            StreamSource<T, ISourceFunction<T>> @operator,
            TypeInformation<T> outputType,
            int parallelism) : this(name, new SimpleOperatorFactory<T>(@operator), outputType, parallelism)
        {

        }

        public SourceTransformation(
            string name,
            IStreamOperatorFactory<T> operatorFactory,
            TypeInformation<T> outputType,
            int parallelism) : base(name, outputType, parallelism)
        {
        }

        public override void SetChainingStrategy(ChainingStrategy strategy)
        {
            throw new NotImplementedException();
        }

        public override IList<Transformation<T>> TransitivePredecessors { get; }
    }
}
