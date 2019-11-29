using FLink.CSharp;

namespace FLink.Clients.Program
{
    /// <summary>
    /// The factory that instantiates the environment to be used when running jobs that are submitted through a pre-configured client connection.
    /// This happens for example when a job is submitted from the command line.
    /// </summary>
    public class ContextEnvironmentFactory : IExecutionEnvironmentFactory
    {
        public ExecutionEnvironment CreateExecutionEnvironment()
        {
            throw new System.NotImplementedException();
        }
    }
}
