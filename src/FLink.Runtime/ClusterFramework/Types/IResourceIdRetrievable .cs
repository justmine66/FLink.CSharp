namespace FLink.Runtime.ClusterFramework.Types
{
    /// <summary>
    /// An interface to retrieve the ResourceId of an object.
    /// </summary>
    public interface IResourceIdRetrievable
    {
        /// <summary>
        /// Gets the ResourceId of the object.
        /// </summary>
        ResourceId ResourceIdentifier { get; }
    }
}
