namespace FLink.Runtime.Events
{
    /// <summary>
    /// Subclasses of this event are recognized as custom events that are not part of the core flink runtime.
    /// </summary>
    public abstract class TaskEvent : AbstractEvent { }
}
