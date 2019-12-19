namespace FLink.Core.Configurations
{
    public interface IReadableConfig
    {
        T Get<T>(ConfigOption<T> option);
    }
}
