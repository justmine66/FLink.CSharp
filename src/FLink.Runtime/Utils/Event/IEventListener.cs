namespace FLink.Runtime.Utils.Event
{
    public interface IEventListener<in TEvent>
    {
        void OnEvent(TEvent @event);
    }
}
