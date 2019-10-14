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

        /// <summary>
        /// Create a <see cref="WindowTriggerResult"/> instance.
        /// </summary>
        /// <param name="fire">Whether to trigger the computation in the window or not.</param>
        /// <param name="purge">Whether to clear the elements in the window or not.</param>
        public WindowTriggerResult(bool fire, bool purge)
        {
            IsFire = fire;
            IsPurge = purge;
        }

        /// <summary>
        /// Whether to trigger the computation in the window or not.
        /// </summary>
        public bool IsFire { get; }

        /// <summary>
        /// Whether to clear the elements in the window or not.
        /// </summary>
        public bool IsPurge { get; }
    }
}
