using System.IO;

namespace FLink.Core.Memory
{
    public class DataInputViewStreamWrapper : IDataInputView
    {
        public DataInputViewStreamWrapper(Stream input)
        {

        }

        public void ReadFully(byte[] bytes)
        {
            throw new System.NotImplementedException();
        }

        public void ReadFully(byte[] bytes, int off, int len)
        {
            throw new System.NotImplementedException();
        }

        public int SkipBytes(int n)
        {
            throw new System.NotImplementedException();
        }

        public bool ReadBool()
        {
            throw new System.NotImplementedException();
        }

        public byte ReadByte()
        {
            throw new System.NotImplementedException();
        }

        public int ReadUnsignedByte()
        {
            throw new System.NotImplementedException();
        }

        public short ReadShort()
        {
            throw new System.NotImplementedException();
        }

        public int ReadUnsignedShort()
        {
            throw new System.NotImplementedException();
        }

        public char ReadChar()
        {
            throw new System.NotImplementedException();
        }

        public int ReadInt()
        {
            throw new System.NotImplementedException();
        }

        public long ReadLong()
        {
            throw new System.NotImplementedException();
        }

        public float ReadFloat()
        {
            throw new System.NotImplementedException();
        }

        public double ReadDouble()
        {
            throw new System.NotImplementedException();
        }

        public string ReadLine()
        {
            throw new System.NotImplementedException();
        }
    }
}
