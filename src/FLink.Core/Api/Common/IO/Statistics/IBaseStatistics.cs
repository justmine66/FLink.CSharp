namespace FLink.Core.Api.Common.IO.Statistics
{
    /// <summary>
    /// Interface describing the basic statistics that can be obtained from the input.
    /// </summary>
    public interface IBaseStatistics
    {
        /// <summary>
        /// Gets the total size of the input.
        /// </summary>
        long TotalInputSize { get; }

        /// <summary>
        /// Gets the number of records in the input (= base cardinality).
        /// </summary>
        long NumberOfRecords { get; }

        /// <summary>
        /// Gets the average width of a record, in bytes.
        /// </summary>
        long AverageRecordWidth { get; }
    }

    public abstract class BaseStatistics : IBaseStatistics
    {
        /// <summary>
        /// Constant indicating that the input size is unknown.
        /// </summary>
        public const long UnknownSize = -1;

        /// <summary>
        /// Constant indicating that the number of records is unknown;
        /// </summary>
        public const long UnknownNumRecords = -1;

        /// <summary>
        /// Constant indicating that average record width is unknown.
        /// </summary>
        public const float UnknownAvgRecordBytes = -1.0f;

        public abstract long TotalInputSize { get; }
        public abstract long NumberOfRecords { get; }
        public abstract long AverageRecordWidth { get; }
    }
}
