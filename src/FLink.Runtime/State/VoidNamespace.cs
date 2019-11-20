namespace FLink.Runtime.State
{
    /// <summary>
    /// Singleton placeholder class for state without a namespace.
    /// </summary>
    public class VoidNamespace
    {
        /// <summary>
        /// The singleton instance
        /// </summary>
        public static readonly VoidNamespace Instance = new VoidNamespace();

        /// <summary>
        /// Getter for the singleton instance
        /// </summary>
        /// <returns></returns>
        public static VoidNamespace Get() => Instance;

        public override bool Equals(object obj) => obj == this;

        public override int GetHashCode() => 99;
    }
}
