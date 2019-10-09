namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// Result type for trigger methods. This determines what happens with the window, for example whether the window function should be called, or the window should be discarded.
    /// </summary>
    public class TriggerResult
    {
        /// <summary>
        /// No action is taken on the window.
        /// </summary>
        public static TriggerResult Continue = new TriggerResult(false, false);

        /// <summary>
        /// Evaluates the window function and emits the window result.
        /// </summary>
        public static TriggerResult FireAndPurge = new TriggerResult(true, true);

        /// <summary>
        /// On fire, the window is evaluated and results are emitted. The window is not purged, though, all elements are retained.
        /// </summary>
        public static TriggerResult Fire = new TriggerResult(true, false);

        /// <summary>
        /// All elements in the window are cleared and the window is discarded, without evaluating the window function or emitting any elements.
        /// </summary>
        public static TriggerResult Purge = new TriggerResult(false, true);

        public TriggerResult(bool fire, bool purge)
        {
            IsFire = fire;
            IsPurge = purge;
        }

        public bool IsFire { get; }
        public bool IsPurge { get; }
    }
}
