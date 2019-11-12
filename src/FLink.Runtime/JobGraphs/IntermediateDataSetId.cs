using FLink.Core.Util;

namespace FLink.Runtime.JobGraphs
{
    public class IntermediateDataSetId : AbstractId
    {
        public IntermediateDataSetId() : base() { }

        /// <summary>
        /// Creates a new intermediate data set ID with the bytes of the given ID.
        /// </summary>
        /// <param name="from">The ID to create this ID from.</param>
        public IntermediateDataSetId(AbstractId from) : base(from) { }
    }
}
