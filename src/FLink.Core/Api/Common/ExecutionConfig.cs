using System;
using FLink.Core.Exceptions;
using FLink.Core.Util;

namespace FLink.Core.Api.Common
{
    /// <summary>
    /// A config to define the behavior of the program execution. It allows to define (among other options) the following settings:
    /// <list>
    ///   <para>The default parallelism of the program, i.e., how many parallel tasks to use for all functions that do not define a specific value directly.</para>
    ///   <para>The number of retries in the case of failed executions.</para>
    ///   <para>The delay between execution retries.</para>
    ///   <para>The <see cref="ExecutionMode"/> of the program: Batch or Pipelined. The default execution mode is <see cref="ExecutionMode.Pipelined"/>.</para>
    ///   <para>Enabling or disabling the "closure cleaner".</para>
    /// </list>
    /// </summary>
    [Serializable]
    public class ExecutionConfig : IArchiveable<ArchivedExecutionConfig>
    {
        /// <summary>
        /// Returns the interval of the automatic watermark emission.
        /// </summary>
        public long AutoWatermarkInterval { get; private set; }

        /// <summary>
        /// Sets the interval of the automatic watermark emission. Watermarks are used throughout the streaming system to keep track of the progress of time. They are used, for example, for time based windowing.
        /// </summary>
        /// <param name="interval">The interval between watermarks in milliseconds.</param>
        /// <returns></returns>
        public ExecutionConfig SetAutoWatermarkInterval(long interval)
        {
            AutoWatermarkInterval = interval;
            return this;
        }

        /// <summary>
        /// The flag value indicating use of the default parallelism.
        /// This value can be used to reset the parallelism back to the default state.
        /// </summary>
        public const int DefaultParallelism = -1;

        /// <summary>
        /// The flag value indicating an unknown or unset parallelism.
        /// This value is not a valid parallelism and indicates that the parallelism should remain unchanged.
        /// </summary>
        public const int UnknownParallelism = -2;

        /// <summary>
        /// Sets the parallelism for operations executed through this environment.
        /// Setting a parallelism of x here will cause all operators (such as join, map, reduce) to run with x parallel instances.
        /// 
        /// This method overrides the default parallelism for this environment.
        /// The local execution environment uses by default a value equal to the number of hardware contexts(CPU cores / threads). When executing the program via the command line client, the default parallelism is the one configured for that setup.
        /// </summary>
        /// <param name="parallelism">The parallelism to use</param>
        /// <returns></returns>
        public ExecutionConfig SetParallelism(int parallelism)
        {
            if (parallelism == UnknownParallelism) return this;
            if (parallelism < 1 && parallelism != DefaultParallelism)
            {
                throw new IllegalArgumentException(
                    "Parallelism must be at least one, or ExecutionConfig.PARALLELISM_DEFAULT (use system default).");
            }

            Parallelism = parallelism;
            return this;
        }

        /// <summary>
        /// Gets the parallelism with which operation are executed by default.
        /// Operations can individually override this value to use a specific parallelism.
        /// 
        /// Other operations may need to run with a different parallelism - for example calling a reduce operation over the entire data set will involve an operation that runs with a parallelism of one (the final reduce to the single result value).
        /// </summary>
        /// <returns>The parallelism used by operations, unless they override that value.This method returns <see cref="DefaultParallelism"/> if the environment's default parallelism should be used.</returns>
        public int Parallelism { get; private set; } = DefaultParallelism;

        private int _maxParallelism = -1;

        /// <summary>
        /// Gets the maximum degree of parallelism defined for the program.
        /// The maximum degree of parallelism specifies the upper limit for dynamic scaling.
        /// It also defines the number of key groups used for partitioned state.
        /// </summary>
        public int MaxParallelism
        {
            get => _maxParallelism;
            set
            {
                Preconditions.CheckArgument(value > 0, "The maximum parallelism must be greater than 0.");

                _maxParallelism = value;
            }
        }

        public ArchivedExecutionConfig Archive()
        {
            throw new NotImplementedException();
        }
    }
}
