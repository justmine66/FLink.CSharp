using FLink.Core.Configurations;

namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// An base interface for all rich user-defined functions. This class defines methods for the life cycle of the functions, as well as methods to access the context in which the functions are executed.
    /// </summary>
    public interface IRichFunction : IFunction
    {
        /// <summary>
        /// Initialization method for the function.
        /// It is called before the actual working methods (like <i>map</i> or <i>join</i>) and thus suitable for one time setup work. For functions that are part of an iteration, this method will be invoked at the beginning of each iteration super step.
        /// The configuration object passed to the function can be used for configuration and initialization. The configuration contains all parameters that were configured on the function in the program composition.
        /// </summary>
        /// <param name="parameters">The configuration containing the parameters attached to the contract.</param>
        /// <exception cref="System.Exception">Implementations may forward exceptions, which are caught by the runtime. When the runtime catches an exception, it aborts the task and lets the fail-over logic decide whether to retry the task execution.</exception>
        void Open(Configuration parameters);

        /// <summary>
        /// Tear-down method for the user code.
        /// It is called after the last call to the main working methods (e.g. <i>map</i> or <i>join</i>). For functions that  are part of an iteration, this method will be invoked after each iteration super step.
        /// </summary>
        /// <exception cref="System.Exception">Implementations may forward exceptions, which are caught by the runtime. When the runtime catches an exception, it aborts the task and lets the fail-over logic decide whether to retry the task execution.</exception>
        void Close();

        /// <summary>
        /// Gets the context that contains information about the UDF's runtime, such as the parallelism of the function, the sub task index of the function, or the name of the of the task that executes the function.
        /// </summary>
        /// <returns>The UDF's runtime context.</returns>
        IRuntimeContext GetRuntimeContext();

        /// <summary>
        /// Sets the function's runtime context. Called by the framework when creating a parallel instance of the function.
        /// </summary>
        /// <param name="cxt">The runtime context.</param>
        void SetRuntimeContext(IRuntimeContext cxt);
    }
}
