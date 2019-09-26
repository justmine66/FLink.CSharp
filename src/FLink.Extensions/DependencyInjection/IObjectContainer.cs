namespace FLink.Extensions.DependencyInjection
{
    public interface IObjectContainer : IObjectProvider, IObjectRegister
    {
        void Build();
    }
}
