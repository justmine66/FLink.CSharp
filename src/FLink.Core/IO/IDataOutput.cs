namespace FLink.Core.IO
{
    public interface IDataOutput
    {
        void Write(int b);

        void Write(byte[] bytes);

        void Write(byte[] bytes, int off, int len);

        void WriteBool(bool v);

        void WriteByte(int v);

        void WriteShort(int v);

        void WriteChar(int v);

        void WriteInt(int v);

        void WriteLong(long v);

        void WriteFloat(float v);

        void WriteDouble(double v);

        void WriteBytes(string s);

        void WriteChars(string s);

        void WriteUTF(string s);
    }
}
