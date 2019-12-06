using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.IO.Statistics;
using FLink.Core.Configurations;
using FLink.Core.IO;
using System.IO;

namespace FLink.Core.Api.Common.IO
{
    /// <summary>
    /// An abstract stub implementation for Rich input formats.
    /// Rich formats have access to their runtime execution context via <see cref="RuntimeContext"/>.
    /// </summary>
    public abstract class RichInputFormat<TRecord, T>: IInputFormat<TRecord, T> where T : IInputSplit
    {
        public IRuntimeContext RuntimeContext { get; set; }

        public abstract void Configure(Configuration parameters);

        public abstract IBaseStatistics GetStatistics(IBaseStatistics cachedStatistics);

        public abstract T[] CreateInputSplits(int minNumSplits);

        public abstract IInputSplitAssigner GetInputSplitAssigner(T[] inputSplits);

        public abstract void Open(T split);

        public abstract bool ReachedEnd { get; }

        public abstract TRecord NextRecord(TRecord reuse);

        public abstract void Close();

        /// <summary>
        /// Opens this InputFormat instance. This method is called once per parallel instance.
        /// Resources should be allocated in this method. (e.g. database connections, cache, etc.)
        /// </summary>
        /// <exception cref="IOException">in case allocating the resources failed.</exception>
        public virtual void OpenInputFormat()
        {
            //do nothing here, just for subclasses
        }

        /// <summary>
        /// Closes this InputFormat instance. This method is called once per parallel instance.
        /// </summary>
        /// <exception cref="IOException">in case closing the resources failed.</exception>
        public virtual void CloseInputFormat()
        {
            //do nothing here, just for subclasses
        }
    }
}
