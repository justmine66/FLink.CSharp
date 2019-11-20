namespace FLink.Core.IO
{
    public interface IDataInput
    {
        void ReadFully(byte[] bytes);

        void ReadFully(byte[] bytes, int off, int len);

        int SkipBytes(int n);

        bool ReadBool();

        byte ReadByte();

        int ReadUnsignedByte();

        short ReadShort();

        int ReadUnsignedShort();

        char ReadChar();

        int ReadInt();

        long ReadLong();

        float ReadFloat();

        double ReadDouble();

        string ReadLine();
    }
}
