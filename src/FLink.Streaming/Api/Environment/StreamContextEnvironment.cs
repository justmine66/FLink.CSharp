namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// Special <see cref="StreamExecutionEnvironment"/> that will be used in cases where the CLI client or testing utilities create a <see cref="StreamExecutionEnvironment"/> that should be used when <see cref="StreamExecutionEnvironment.GetExecutionEnvironment()"/> is called.
    /// </summary>
    public class StreamContextEnvironment : StreamExecutionEnvironment
    {

    }
}
