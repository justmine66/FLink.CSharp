using System;
using FLink.Core.Api.Common.Functions;

namespace FLink.Runtime.Operators
{
    /// <summary>
    /// The interface to be implemented by all drivers that run alone (or as the primary driver) in a task.
    /// </summary>
    /// <typeparam name="TFunction">The type of stub driven by this driver.</typeparam>
    /// <typeparam name="TData">The data type of the records produced by this driver.</typeparam>
    public interface IDriver<TFunction, TData> where TFunction : IFunction
    {
        /// <summary>
        /// Gets the number of inputs that the task has.
        /// </summary>
        int NumberOfInputs { get; }

        /// <summary>
        /// Gets the number of comparators required for this driver.
        /// </summary>
        int NumberOfDriverComparators { get; }

        /// <summary>
        /// Gets the class of the stub type that is run by this task. For example, a <tt>MapTask</tt> should return <code>MapFunction.class</code>.   
        /// </summary>
        Type StubType { get; }

        void Setup(ITaskContext<TFunction, TData> context);

        /// <summary>
        /// This method is called before the user code is opened. An exception thrown by this method signals failure of the task.
        /// </summary>
        /// <exception cref="Exception">Exceptions may be forwarded and signal task failure.</exception>
        void PrePare();

        /// <summary>
        /// The main operation method of the task. It should call the user code with the data subsets until the input is depleted.
        /// </summary>
        /// <exception cref="Exception">Any exception thrown by this method signals task failure. Because exceptions in the user code typically signal situations where this instance in unable to proceed, exceptions from the user code should be forwarded.</exception>
        void Run();

        /// <summary>
        /// This method is invoked in any case (clean termination and exception) at the end of the tasks operation.
        /// </summary>
        /// <exception cref="Exception">Exceptions may be forwarded.</exception>
        void Cleanup();

        /// <summary>
        /// This method is invoked when the driver must aborted in mid processing. It is invoked asynchronously by a different thread.
        /// </summary>
        /// <exception cref="Exception">Exceptions may be forwarded.</exception>
        void Cancel();
    }
}
