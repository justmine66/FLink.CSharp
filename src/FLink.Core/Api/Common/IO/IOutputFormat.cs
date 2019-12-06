using FLink.Core.Configurations;
using System.IO;

namespace FLink.Core.Api.Common.IO
{
    /// <summary>
    /// The base interface for outputs that consumes records.
    /// The output format describes how to store the final records, for example in a file.
    /// The life cycle of an output format is the following:
    /// 1.configure() is invoked a single time. The method can be used to implement initialization from the parameters (configuration) that may be attached upon instantiation.
    /// 2.Each parallel output task creates an instance, configures it and opens it.
    /// 3.All records of its parallel instance are handed to the output format.
    /// 4.The output format is closed
    /// </summary>
    /// <typeparam name="TRecord">The type of the consumed records. </typeparam>
    public interface IOutputFormat<in TRecord>
    {
        /// <summary>
        /// Configures this output format. Since output formats are instantiated generically and hence parameterless, this method is the place where the output formats set their basic fields based on configuration values.
        /// This method is always called first on a newly instantiated output format. 
        /// </summary>
        /// <param name="parameters">The configuration with all parameters.</param>
        void Configure(Configuration parameters);

        /// <summary>
        /// Opens a parallel instance of the output format to store the result of its parallel instance.
        /// When this method is called, the output format it guaranteed to be configured.
        /// </summary>
        /// <param name="taskNumber">The number of the parallel instance.</param>
        /// <param name="numTasks">The number of parallel tasks.</param>
        /// <exception cref="IOException">Thrown, if the output could not be opened due to an I/O problem.</exception>
        void Open(int taskNumber, int numTasks);

        /// <summary>
        /// Adds a record to the output.
        /// When this method is called, the output format it guaranteed to be opened.
        /// </summary>
        /// <param name="record">The records to add to the output.</param>
        /// <exception cref="IOException">Thrown, if the records could not be added to to an I/O problem.</exception>
        void WriteRecord(TRecord record);

        /// <summary>
        /// Method that marks the end of the life-cycle of parallel output instance.
        /// Should be used to close channels and streams and release resources.
        /// After this method returns without an error, the output is assumed to be correct.
        /// When this method is called, the output format it guaranteed to be opened.
        /// </summary>
        /// <exception cref="IOException">Thrown, if the input could not be closed properly.</exception>
        void Close();
    }
}
