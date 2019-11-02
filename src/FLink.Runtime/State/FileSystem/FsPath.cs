namespace FLink.Runtime.State.FileSystem
{
    public class FsPath
    {
        public FsPath(string fullPath) => FullPath = fullPath;
        public FsPath(string scheme, string path)
        {
            Scheme = scheme;
            Path = path;
            FullPath = $"{scheme}{path}";
        }

        public string Scheme { get; }
        public string Path { get; }
        public string FullPath { get; }
    }
}
