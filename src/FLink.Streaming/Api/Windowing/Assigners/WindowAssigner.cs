using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{T,TW}"/> assigns zero or more windows to an element.
    /// </summary>
    /// <typeparam name="T">The type of elements that this WindowAssigner can assign windows to.</typeparam>
    /// <typeparam name="TW">The type of <see cref="Window"/> that this assigner assigns.</typeparam>
    public abstract class WindowAssigner<T, TW> where TW : Window
    {

    }
}
