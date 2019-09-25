using System;

namespace FLink.Streaming.Api.Windowing.Windows
{
    /// <summary>
    /// The default window into which all data is placed (via <see cref="GlobalWindow"/>).
    /// </summary>
    public class GlobalWindow : Window, IEquatable<GlobalWindow>
    {
        private static readonly GlobalWindow Instance = new GlobalWindow();

        private GlobalWindow() { }

        public static GlobalWindow Get() => Instance;

        public override long MaxTimestamp() => long.MaxValue;

        public bool Equals(GlobalWindow other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;

            return true;
        }

        public override bool Equals(object obj) => obj is GlobalWindow other && Equals(other);

        public override int GetHashCode() => 0;

        public override string ToString() => "GlobalWindow";
    }
}
