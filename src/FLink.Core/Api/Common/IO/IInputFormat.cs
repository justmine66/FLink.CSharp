using System.IO;
using FLink.Core.Api.Common.IO.Statistics;
using FLink.Core.Configurations;
using FLink.Core.IO;

namespace FLink.Core.Api.Common.IO
{
    /// <summary>
    /// The base interface for data sources that produces records.
    /// The input format handles the following:
    /// 1. It describes how the input is split into splits that can be processed in parallel.
    /// 2. It describes how to read records from the input split.
    /// 3. It describes how to gather basic statistics from the input.
    /// The life cycle of an input format is the following:
    /// 1. After being instantiated (parameterless), it is configured with a <see cref="Configuration"/> object. Basic fields are read from the configuration, such as a file path, if the format describes files as input.
    /// 2. Optionally: It is called by the compiler to produce basic statistics about the input.
    /// 3. It is called to create the input splits.
    /// 4. Each parallel input task creates an instance, configures it and opens it for a specific split.
    /// 5. All records are read from the input.
    /// 6. The input format is closed.
    /// IMPORTANT NOTE: Input formats must be written such that an instance can be opened again after it was closed. That is due to the fact that the input format is used for potentially multiple splits. After a split is done, the format's close function is invoked and, if another split is available, the open function is invoked afterwards for the next split.
    /// </summary>
    /// <typeparam name="TRecord">The type of the produced records.</typeparam>
    /// <typeparam name="TInputSplit">The type of input split.</typeparam>
    public interface IInputFormat<TRecord, TInputSplit> : IInputSplitSource<TInputSplit> where TInputSplit : IInputSplit
    {
        /// <summary>
        /// Configures this input format. Since input formats are instantiated generically and hence parameterless, this method is the place where the input formats set their basic fields based on configuration values.
        /// This method is always called first on a newly instantiated input format. 
        /// </summary>
        /// <param name="parameters">The configuration with all parameters (note: not the Flink config but the TaskConfig).</param>
        void Configure(Configuration parameters);

        /// <summary>
        /// Gets the basic statistics from the input described by this format. If the input format does not know how to create those statistics, it may return null.
        /// This method optionally gets a cached version of the statistics. The input format may examine them and decide whether it directly returns them without spending effort to re-gather the statistics.
        /// When this method is called, the input format it guaranteed to be configured.
        /// </summary>
        /// <param name="cachedStatistics">The statistics that were cached. May be null.</param>
        /// <returns>The base statistics for the input, or null, if not available.</returns>
        /// <exception cref="IOException">Thrown, if an I/O error occurred.</exception>
        IBaseStatistics GetStatistics(IBaseStatistics cachedStatistics);

        /// <summary>
        /// Creates the different splits of the input that can be processed in parallel.
        /// When this method is called, the input format it guaranteed to be configured.
        /// </summary>
        /// <param name="minNumSplits">The minimum desired number of splits. If fewer are created, some parallel instances may remain idle.</param>
        /// <returns>The splits of this input that can be processed in parallel. </returns>
        /// <exception cref="IOException">Thrown, when the creation of the splits was erroneous.</exception>
        new TInputSplit[] CreateInputSplits(int minNumSplits);

        /// <summary>
        /// Gets the type of the input splits that are processed by this input format.
        /// </summary>
        /// <param name="inputSplits"></param>
        /// <returns>The type of the input splits.</returns>
        new IInputSplitAssigner GetInputSplitAssigner(TInputSplit[] inputSplits);

        /// <summary>
        /// Opens a parallel instance of the input format to work on a split.
        /// When this method is called, the input format it guaranteed to be configured.
        /// </summary>
        /// <param name="split">The split to be opened.</param>
        /// <exception cref="IOException">Thrown, if the spit could not be opened due to an I/O problem.</exception>
        void Open(TInputSplit split);

        /// <summary>
        /// Method used to check if the end of the input is reached.
        /// When this method is called, the input format it guaranteed to be opened.
        /// </summary>
        /// <exception cref="IOException">Thrown, if an I/O error occurred.</exception>
        bool ReachedEnd { get; }

        /// <summary>
        /// Reads the next record from the input.
        /// When this method is called, the input format it guaranteed to be opened.
        /// </summary>
        /// <param name="reuse">Object that may be reused.</param>
        /// <returns>Read record.</returns>
        /// <exception cref="IOException">Thrown, if an I/O error occurred.</exception>
        TRecord NextRecord(TRecord reuse);

        /// <summary>
        /// Method that marks the end of the life-cycle of an input split. Should be used to close channels and streams and release resources.After this method returns without an error, the input is assumed to be correctly read.
        /// When this method is called, the input format it guaranteed to be opened.
        /// </summary>
        /// <exception cref="IOException">Thrown, if an I/O error occurred.</exception>
        void Close();
    }
}
