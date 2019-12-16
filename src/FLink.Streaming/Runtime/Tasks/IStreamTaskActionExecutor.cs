using System;
using FLink.Core.Api.CSharp.Concurrent;
using FLink.Core.Api.CSharp.Threading;
using FLink.Core.Util.Function;

namespace FLink.Streaming.Runtime.Tasks
{
    /// <summary>
    /// Executes <see cref="IRunnable"/>, <see cref="IThrowingRunnable{TException}"/>, or <see cref="ICallable{TResult}"/>.
    /// </summary>
    public interface IStreamTaskActionExecutor
    {
        void Run(IRunnableWithException runnable);

        void RunThrowing<T>(IThrowingRunnable<T> runnable) where T : Exception;

        TResult Call<TResult>(ICallable<TResult> callable);
    }

    public class StreamTaskActionExecutor : IStreamTaskActionExecutor
    {
        public virtual void Run(IRunnableWithException runnable) => runnable.Run();

        public virtual void RunThrowing<T>(IThrowingRunnable<T> runnable) where T : Exception => runnable.Run();

        public virtual TResult Call<TResult>(ICallable<TResult> callable) => callable.Call();
    }

    public class SynchronizedStreamTaskActionExecutor : StreamTaskActionExecutor
    {
        /// <summary>
        /// an object used for mutual exclusion of all operations that involve data and state mutation.
        /// </summary>
        public object Mutex { get; }

        public SynchronizedStreamTaskActionExecutor(object mutex)
        {
            Mutex = mutex;
        }

        public override void Run(IRunnableWithException runnable)
        {
            lock (Mutex)
            {
                runnable.Run();
            }
        }

        public override void RunThrowing<T>(IThrowingRunnable<T> runnable)
        {
            lock (Mutex)
            {
                runnable.Run();
            }
        }

        public override TResult Call<TResult>(ICallable<TResult> callable)
        {
            lock (Mutex)
            {
                return callable.Call();
            }
        }
    }
}
