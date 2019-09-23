using System.IO;

namespace FLink.Core.Memory
{
    public class DataInputViewStreamWrapper : MemoryStream, IDataInputView
    {
        public DataInputViewStreamWrapper(Stream input)
        {
            
        }
    }
}
