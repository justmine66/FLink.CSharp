namespace FLink.Streaming.Api.Functions.Source
{
    public enum FileProcessingMode
    {
        /// <summary>
        /// Processes the current contents of the path and exits.
        /// </summary>
        ProcessOnce,

        /// <summary>
        /// Periodically scans the path for new data.
        /// </summary>
        ProcessContinuously
    }
}
