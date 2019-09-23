using System.IO;

namespace FLink.Core.Memory
{
    public class DataOutputViewStreamWrapper : MemoryStream, IDataOutputView
    {
        public DataOutputViewStreamWrapper(Stream input)
        {
            
        }
    }
}
