namespace FLink.Core.Configurations
{
    public class DeploymentOptions
    {
        public static readonly ConfigOption<string> Target = ConfigOptionBuilder
            .Key("execution.target")
            .StringType
            .NoDefaultValue()
            .WithDescription("The deployment target for the execution, e.g. \"local\" for local execution.");

        public static readonly ConfigOption<bool> Attached = ConfigOptionBuilder
            .Key("execution.attached")
            .BoolType
            .NoDefaultValue()
            .WithDescription("Specifies if the pipeline is submitted in attached or detached mode.");

        public static readonly ConfigOption<bool> ShutdownIfAttached = ConfigOptionBuilder
            .Key("execution.shutdown-on-attached-exit")
            .BoolType
            .NoDefaultValue()
            .WithDescription("If the job is submitted in attached mode, perform a best-effort cluster shutdown when the CLI is terminated abruptly, e.g., in response to a user interrupt, such as typing Ctrl + C.");
    }
}
