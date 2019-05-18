using FLink.Core.Api.Common.State;

namespace FLink.Runtime.State
{
    /// <summary>
    /// This interface provides a context in which operators can initialize by registering to managed state (i.e. state that is managed by state backends).
    /// Operator state is available to all operators, while keyed state is only available for operators after keyBy.
    /// For the purpose of initialization, the context signals if the state is empty (new operator) or was restored from a previous execution of this operator.
    /// </summary>
    public interface IManagedInitializationContext
    {
        /// <summary>
        /// Returns true, if state was restored from the snapshot of a previous execution. This returns always false for stateless tasks.
        /// </summary>
        bool IsRestored { get; }

        /// <summary>
        /// Returns an interface that allows for registering operator state with the backend.
        /// </summary>
        IOperatorStateStore OperatorStateStore { get; }

        /// <summary>
        /// Returns an interface that allows for registering keyed state with the backend.
        /// </summary>
        IKeyedStateStore KeyedStateStore { get; }
    }
}
