using FLink.Core.Api.CSharp.Threading;
using System;

namespace FLink.Core.Util.Function
{
    /// <summary>
    /// Similar to a <see cref="IRunnable"/>, this interface is used to capture a block of code to be executed. In contrast to <see cref="IRunnable"/>, this interface allows throwing checked exceptions.
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    public interface IThrowingRunnable<TException> : IRunnable where TException : Exception
    {
        /// <summary>
        /// The work method.
        /// </summary>
        /// <exception cref="Exception">Exceptions may be thrown.</exception>
        new void Run();
    }

    public abstract class ThrowingRunnable<TException> : IThrowingRunnable<TException> where TException : Exception
    {
        public abstract void Run();

        public static IRunnable UnChecked(IThrowingRunnable<TException> throwingRunnable)
        {
            try
            {
                 throwingRunnable.Run();
            }
            catch (Exception)
            {
                // nothing
            }

            return throwingRunnable;
        }
    }
}
