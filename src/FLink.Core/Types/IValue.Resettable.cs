namespace FLink.Core.Types
{
    public interface IResettableValue<in T> : IValue where T : IValue
    {
        /// <summary>
        /// Sets the encapsulated value to another value 
        /// </summary>
        /// <param name="value">the new value of the encapsulated value</param>
        void SetValue(T value);
    }
}
