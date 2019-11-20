using System.IO;

namespace FLink.Core.Memory
{
    public class DataOutputViewStreamWrapper : IDataOutputView
    {
        public DataOutputViewStreamWrapper(Stream input)
        {

        }

        public void Write(int b)
        {
            throw new System.NotImplementedException();
        }

        public void Write(byte[] bytes)
        {
            throw new System.NotImplementedException();
        }

        public void Write(byte[] bytes, int off, int len)
        {
            throw new System.NotImplementedException();
        }

        public void WriteBool(bool v)
        {
            throw new System.NotImplementedException();
        }

        public void WriteByte(int v)
        {
            throw new System.NotImplementedException();
        }

        public void WriteShort(int v)
        {
            throw new System.NotImplementedException();
        }

        public void WriteChar(int v)
        {
            throw new System.NotImplementedException();
        }

        public void WriteInt(int v)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLong(long v)
        {
            throw new System.NotImplementedException();
        }

        public void WriteFloat(float v)
        {
            throw new System.NotImplementedException();
        }

        public void WriteDouble(double v)
        {
            throw new System.NotImplementedException();
        }

        public void WriteBytes(string s)
        {
            throw new System.NotImplementedException();
        }

        public void WriteChars(string s)
        {
            throw new System.NotImplementedException();
        }

        public void WriteUTF(string s)
        {
            throw new System.NotImplementedException();
        }

        public void SkipBytesToWrite(int numBytes)
        {
            throw new System.NotImplementedException();
        }

        public void Write(IDataInputView source, int numBytes)
        {
            throw new System.NotImplementedException();
        }
    }
}
