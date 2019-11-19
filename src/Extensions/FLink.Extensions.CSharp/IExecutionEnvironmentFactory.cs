namespace FLink.Extensions.CSharp
{
    /// <summary>
    /// Factory class for execution environments.
    /// </summary>
    public interface IExecutionEnvironmentFactory
    {
        /// <summary>
        /// Creates an ExecutionEnvironment from this factory.
        /// </summary>
        /// <returns>An ExecutionEnvironment.</returns>
        ExecutionEnvironment CreateExecutionEnvironment();
    }
}
