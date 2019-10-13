namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// Result type for trigger methods. This determines what happens with the window, for example whether the window function should be called, or the window should be discarded.
    /// </summary>
    public class WindowTriggerResult
    {
        /// <summary>
        /// No action is taken on the window.
        /// </summary>
        public static WindowTriggerResult Continue = new WindowTriggerResult(false, false);

        /// <summary>
        /// Evaluates the window function and emits the window result.
        /// </summary>
        public static WindowTriggerResult FireAndPurge = new WindowTriggerResult(true, true);

        /// <summary>
        /// On fire, the window is evaluated and results are emitted. The window is not purged, though, all elements are retained.
        /// </summary>
        public static WindowTriggerResult Fire = new WindowTriggerResult(true, false);

        /// <summary>
        /// All elements in the window are cleared and the window is discarded, without evaluating the window function or emitting any elements.
        /// </summary>
        public static WindowTriggerResult Purge = new WindowTriggerResult(false, true);

        public WindowTriggerResult(bool fire, bool purge)
        {
            IsFire = fire;
            IsPurge = purge;
        }

        public bool IsFire { get; }
        public bool IsPurge { get; }
    }
}
