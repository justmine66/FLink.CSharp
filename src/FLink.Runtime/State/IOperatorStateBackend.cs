using System;
using FLink.Core.Api.Common.State;
using FLink.Core.IO;

namespace FLink.Runtime.State
{
    /// <summary>
    /// Interface that combines both, the user facing <see cref="IOperatorStateStore"/> interface and the system interface <see cref="ISnapshotStrategy{TState}"/>.
    /// </summary>
    public interface IOperatorStateBackend : IOperatorStateStore, ISnapshotStrategy<SnapshotResult<IOperatorStateHandle>>,ICloseable, IDisposable
    {

    }
}
