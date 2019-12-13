namespace FLink.Runtime.Utils.Event
{
    public interface IEventListener<in T>
    {
        void OnEvent(T @event);
    }
}
