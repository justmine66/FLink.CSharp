namespace FLink.Core.Api.CSharp.Concurrent
{
    public interface ICallable<out TResult>
    {
        /// <summary>
        /// Computes a result, or throws an exception if unable to do so.
        /// </summary>
        /// <returns>computed result</returns>
        /// <exception cref="">if unable to compute a result</exception>
        TResult Call();
    }
}
