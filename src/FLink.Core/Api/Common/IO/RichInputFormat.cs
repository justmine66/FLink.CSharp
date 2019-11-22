using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.IO.Statistics;
using FLink.Core.Configurations;
using FLink.Core.IO;

namespace FLink.Core.Api.Common.IO
{
    /// <summary>
    /// An abstract stub implementation for Rich input formats.
    /// Rich formats have access to their runtime execution context via <see cref="RuntimeContext"/>.
    /// </summary>
    public abstract class RichInputFormat<OT, T>: IInputFormat<OT, T> where T : IInputSplit
    {
        public IRuntimeContext RuntimeContext { get; set; }

        public abstract void Configure(Configuration parameters);

        public abstract IBaseStatistics GetStatistics(IBaseStatistics cachedStatistics);

        public abstract T[] CreateInputSplits(int minNumSplits);

        public abstract IInputSplitAssigner GetInputSplitAssigner(T[] inputSplits);

        public abstract void Open(T split);

        public abstract bool ReachedEnd { get; }

        public abstract OT NextRecord(OT reuse);

        public abstract void Close();
    }
}
