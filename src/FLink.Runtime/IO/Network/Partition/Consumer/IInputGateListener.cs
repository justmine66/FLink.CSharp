namespace FLink.Runtime.IO.Network.Partition.Consumer
{
    /// <summary>
    /// Listener interface implemented by consumers of <see cref="IInputGate"/> instances that want to be notified of availability of buffer or event instances.
    /// </summary>
    public interface IInputGateListener
    {
        /// <summary>
        /// Notification callback if the input gate moves from zero to non-zero available input channels with data.
        /// </summary>
        /// <param name="inputGate">Input Gate that became available.</param>
        void NotifyInputGateNonEmpty(IInputGate inputGate);
    }
}