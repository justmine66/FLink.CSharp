namespace FLink.Runtime.State
{
    public interface IStateObject
    {
        void DiscardState();
        void GetStateSize();
    }
}
