using FLink.Core.Util;

namespace FLink.Runtime.JobGraphs
{
    /// <summary>
    /// A class for statistically unique job vertex IDs.
    /// </summary>
    public class JobVertexId : AbstractId
    {
        public JobVertexId() : base() { }

        public JobVertexId(byte[] bytes) : base(bytes) { }

        public JobVertexId(long lowerPart, long upperPart) : base(lowerPart, upperPart) { }

        public static JobVertexId FromHexString(string hexString) => new JobVertexId(StringUtils.HexStringToByte(hexString));
    }
}
