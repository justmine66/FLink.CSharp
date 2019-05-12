namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// An base interface for all rich user-defined functions. This class defines methods for the life cycle of the functions, as well as methods to access the context in which the functions are executed.
    /// </summary>
    public interface IRichFunction : IFunction
    {
        /// <summary>
        /// Initialization method for the function.
        /// </summary>
        /// <param name="parameters"></param>
        void Open(Configuration.Configuration parameters);

        /// <summary>
        /// Tear-down method for the user code.
        /// </summary>
        void Close();
    }
}
