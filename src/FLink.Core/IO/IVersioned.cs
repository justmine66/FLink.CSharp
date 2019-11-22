namespace FLink.Core.IO
{
    /// <summary>
    /// This interface is implemented by classes that provide a version number. Versions numbers can be used to differentiate between evolving classes.
    /// </summary>
    public interface IVersioned
    {
        /// <summary>
        /// Gets the version number of the object.
        /// Versions numbers can be used to differentiate evolving classes.
        /// </summary>
        int Version { get; }
    }
}
