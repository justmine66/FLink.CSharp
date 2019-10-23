using System;
using FLink.Core.IO;

namespace FLink.Runtime.IO.Network.Api.Writer
{
    /// <summary>
    /// Determines to which logical channels a record should be written to.
    /// </summary>
    /// <typeparam name="TRecord">the type of record which is sent through the attached output gate</typeparam>
    public interface IChannelSelector<in TRecord> where TRecord : IIOReadableWritable
    {
        /// <summary>
        /// Initializes the channel selector with the number of output channels.
        /// </summary>
        /// <param name="numberOfChannels">the total number of output channels which are attached to respective output gate.</param>
        void Setup(int numberOfChannels);

        /// <summary>
        /// Returns the logical channel index, to which the given record should be written. It is illegal to call this method for broadcast channel selectors and this method can remain not implemented in that case (for example by throwing <see cref="InvalidOperationException"/>).
        /// </summary>
        /// <param name="record">the record to determine the output channels for.</param>
        /// <returns>an integer number which indicates the index of the output channel through which the record shall be forwarded.</returns>
        int SelectChannel(TRecord record);

        /// <summary>
        /// true if the selector is for broadcast mode.
        /// Returns whether the channel selector always selects all the output channels.
        /// </summary>
        bool IsBroadcast { get; }
    }
}
