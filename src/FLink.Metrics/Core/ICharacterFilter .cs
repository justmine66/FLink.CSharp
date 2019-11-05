namespace FLink.Metrics.Core
{
    /// <summary>
    /// Interface for a character filter function. The filter function is given a string which the filter can transform. The returned string is the transformation result.
    /// </summary>
    public interface ICharacterFilter
    {
        /// <summary>
        /// Filter the given string and generate a resulting string from it.
        /// For example, one implementation could filter out invalid characters from the input string.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Filtered result string</returns>
        string FilterCharacters(string input);
    }
}
