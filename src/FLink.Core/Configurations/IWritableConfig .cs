namespace FLink.Core.Configurations
{
    /// <summary>
    /// Write access to a configuration object.  
    /// </summary>
    public interface IWritableConfig
    {
        IWritableConfig Set<T>(ConfigOption<T> option, T value);
    }
}
