using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Api.Windowing.Triggers;

namespace FLink.Streaming.Api.Windowing.Evictors
{
    /// <summary>
    /// An evictor can remove elements from a pane before/after the evaluation of WindowFunction and after the window evaluation gets triggered by a <see cref="Trigger{T,TW}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements that this evictor can evict.</typeparam>
    /// <typeparam name="TW">The type of window on which this evictor can operate.</typeparam>
    public interface IEvictor<T, TW> where TW : Window
    {

    }
}
