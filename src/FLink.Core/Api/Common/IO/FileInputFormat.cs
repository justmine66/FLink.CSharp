using FLink.Core.FS;

namespace FLink.Core.Api.Common.IO
{
    /// <summary>
    /// The base class for <see cref="RichInputFormat{TRecord,T}"/>s that read from files.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class FileInputFormat<TRecord> : RichInputFormat<TRecord, FileInputSplit>
    {

    }
}
