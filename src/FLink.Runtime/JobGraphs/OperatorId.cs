using FLink.Core.Util;

namespace FLink.Runtime.JobGraphs
{
    /// <summary>
    /// A class for statistically unique operator IDs.
    /// </summary>
    public class OperatorId : AbstractId
    {
        public OperatorId() : base() { }

        public OperatorId(byte[] bytes) : base(bytes) { }

        public OperatorId(long lowerPart, long upperPart) : base(lowerPart, upperPart) { }

        public static OperatorId FromJobVertexId(JobVertexId id) => new OperatorId(id.LowerPart, id.UpperPart);
    }
}
