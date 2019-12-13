using FLink.Core.IO;
using FLink.Core.Memory;
using FLink.Runtime.TaskExecutors;

namespace FLink.Runtime.Events
{
    /// <summary>
    /// This type of event can be used to exchange notification messages between different <see cref="TaskExecutor"/> objects at runtime using the communication channels.
    /// </summary>
    public abstract class AbstractEvent : IIOReadableWritable
    {
        public abstract void Write(IDataOutputView output);

        public abstract void Read(IDataInputView input);
    }
}
