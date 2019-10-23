using FLink.Core.Memory;

namespace FLink.Core.IO
{
    /// <summary>
    /// This interface must be implemented by every class whose objects have to be serialized to their binary representation and vice-versa.
    /// In particular, records have to implement this interface in order to specify how their data can be transferred to a binary representation.
    /// When implementing this Interface make sure that the implementing class has a default (zero-argument) constructor!
    /// </summary>
    public interface IIOReadableWritable
    {
        /// <summary>
        /// Writes the object's internal data to the given data output view.
        /// </summary>
        /// <param name="output">the output view to receive the data.</param>
        /// <exception cref="System.IO.IOException">thrown if any error occurs while writing to the output stream.</exception>
        void Write(IDataOutputView output);

        /// <summary>
        /// Reads the object's internal data from the given data input view.
        /// </summary>
        /// <param name="input">the input view to read the data from</param>
        /// <exception cref="System.IO.IOException">thrown if any error occurs while reading from the input stream.</exception>
        void Read(IDataInputView input);
    }
}
