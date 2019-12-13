using FLink.Core.Util;
using FLink.Runtime.Topologies;

namespace FLink.Runtime.JobGraphs
{
    public class IntermediateResultPartitionId : AbstractId, IResultId
    {
        /// <summary>
        /// Creates an new random intermediate result partition ID.
        /// </summary>
        public IntermediateResultPartitionId()
            : base()
        { }

        public IntermediateResultPartitionId(long lowerPart, long upperPart)
            : base(lowerPart, upperPart)
        { }
    }
}
