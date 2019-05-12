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
    public class ExecutionConfig
    {

    }
}
