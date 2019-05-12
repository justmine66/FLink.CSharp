using FLink.Streaming.Api.Watermark;

namespace FLink.Streaming.Runtime.StreamRecord
{
    /// <summary>
    /// An element in a data stream. Can be a record or a Watermark.
    /// </summary>
    public abstract class StreamElement
    {
        /// <summary>
        /// Checks whether this element is a watermark.
        /// </summary>
        public bool IsWatermark => GetType().IsAssignableFrom(typeof(Watermark));

        /// <summary>
        /// Casts this element into a Watermark.
        /// </summary>
        /// <returns>This element as a Watermark.</returns>
        /// <exception cref="System.InvalidCastException">Thrown, if this element is actually not a Watermark.</exception>
        public Watermark AsWatermark()
        {
            return (Watermark)this;
        }
    }
}
