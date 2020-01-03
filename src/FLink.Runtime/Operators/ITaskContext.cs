namespace FLink.Runtime.Operators
{
    /// <summary>
    ///  The task context gives a driver access to the runtime components and configuration that they can use to fulfil their task.
    /// </summary>
    /// <typeparam name="TUdf">The UDF type.</typeparam>
    /// <typeparam name="TData">The produced data type.</typeparam>
    public interface ITaskContext<TUdf, TData>
    {

    }
}
