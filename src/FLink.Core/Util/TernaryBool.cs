namespace FLink.Core.Util
{
    /// <summary>
    /// A ternary boolean, which can have the values 'true', 'false', or 'undefined'.
    /// </summary>
    public enum TernaryBool
    {
        True, False, Undefined
    }

    public static class TernaryBoolExtensions
    {
        public static TernaryBool FromBool(bool input) => input ? TernaryBool.True : TernaryBool.False;
        public static TernaryBool FromBool(this TernaryBool it, bool input) => input ? TernaryBool.True : TernaryBool.False;
        public static TernaryBool FromBoxedBool(this TernaryBool it, bool? input) => input == null ? TernaryBool.Undefined : it.FromBool(input.Value);
        public static TernaryBool AsTernaryBool(this bool input) => input ? TernaryBool.True : TernaryBool.False;
        public static TernaryBool AsTernaryBool(this bool? input) => input == null ? TernaryBool.Undefined : FromBool(input.Value);
        public static bool GetOrDefault(this TernaryBool it, bool defaultValue) => it == TernaryBool.Undefined ? defaultValue : it == TernaryBool.True;
    }
}
