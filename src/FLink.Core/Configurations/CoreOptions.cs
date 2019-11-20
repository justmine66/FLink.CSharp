namespace FLink.Core.Configurations
{
    /// <summary>
    /// The set of configuration options for core parameters.
    /// </summary>
    public class CoreOptions
    {
        public static ConfigOption<int> DefaultParallelism = ConfigOptionBuilder
            .Key("parallelism.default")
            .DefaultValue(1)
            .WithDescription("Default parallelism for jobs.");
    }
}
