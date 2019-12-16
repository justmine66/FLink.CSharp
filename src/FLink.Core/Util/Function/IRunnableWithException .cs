using System;
using FLink.Core.Api.CSharp.Threading;

namespace FLink.Core.Util.Function
{
    /// <summary>
    /// Similar to a <see cref="IRunnable"/>, this interface is used to capture a block of code to be executed.
    /// In contrast to <see cref="IRunnable"/>, this interface allows throwing checked exceptions.
    /// </summary>
    public interface IRunnableWithException : IThrowingRunnable<Exception>
    { 
        /// <summary>
        /// The work method.
        /// </summary>
        /// <exception cref="Exception">Exceptions may be thrown.</exception>
        new void Run();
    }
}
