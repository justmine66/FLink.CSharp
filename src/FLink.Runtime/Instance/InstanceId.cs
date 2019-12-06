using FLink.Core.Util;

namespace FLink.Runtime.Instance
{
    /// <summary>
    /// Class for statistically unique instance IDs.
    /// </summary>
    public class InstanceId : AbstractId
    {
        public InstanceId()
        {
        }

        public InstanceId(byte[] bytes) 
            : base(bytes)
        {
        }
    }
}
