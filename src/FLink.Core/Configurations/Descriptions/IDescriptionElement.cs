namespace FLink.Core.Configurations.Descriptions
{
    /// <summary>
    /// Part of a <see cref="Description"/> that can be converted into String representation.
    /// </summary>
    public interface IDescriptionElement
    {
        /// <summary>
        /// Transforms itself into String representation using given format.
        /// </summary>
        /// <param name="formatter">formatter to use.</param>
        void Format(Formatter formatter);
    }
}
