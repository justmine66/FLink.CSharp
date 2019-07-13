namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// An abstract stub implementation for rich user-defined functions.
    /// </summary>
    public abstract class AbstractRichFunction : IRichFunction
    {
        public IRuntimeContext RuntimeContext { get; set; }

        public abstract void Open(Configuration.Configuration parameters);

        public abstract void Close();
    }
}
