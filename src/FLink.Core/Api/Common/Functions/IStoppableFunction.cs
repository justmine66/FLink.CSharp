namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// Must be implemented by stoppable functions, eg, source functions of streaming jobs. The method <see cref="Stop"/> will
    /// be called when the job received the STOP signal.On this signal, the source function must stop emitting new data and
    /// terminate gracefully.
    /// </summary>
    public interface IStoppableFunction
    {
        /// <summary>
        /// Stops the source. In contrast to the method Cancel() this is a request to the source function to shut down
        /// gracefully.Pending data can still be emitted and it is not required to stop immediately -- however, in the near
        /// future.The job will keep running until all emitted data is processed completely.
        ///
        /// Most streaming sources will have a while loop inside the source function Run() method. You need to ensure that the source
        /// will break out of this loop.This can be achieved by having a volatile field "isRunning" that is checked in the
        /// loop and that is set to false in this method.
        ///
        /// The call to <see cref="Stop"/> should not block and not throw any exception.
        /// </summary>
        void Stop();
    }
}
