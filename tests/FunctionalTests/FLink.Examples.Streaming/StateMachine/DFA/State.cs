using FLink.Examples.Streaming.StateMachine.Events;

namespace FLink.Examples.Streaming.StateMachine.DFA
{
    /// <summary>
    /// The State captures the main functionality of the state machine.
    /// It represents a specific state in the state machine, and holds all transitions possible from a specific state.
    /// The state transition diagram is as follows:
    ///           +--[a]--> W --[b]--> Y --[e]---+
    ///           |                    ^         |
    ///   Initial-+                    |         |
    ///           |                    |         +--> (Z)-----[g]---> Terminal
    ///           +--[c]--> X --[b]----+         |
    ///                     |                    |
    ///                     +--------[d]---------+
    /// </summary>
    public class State
    {
        public static State Terminal = new State("Terminal");
        public static State InvalidTransition = new State("InvalidTransition");
        public static State Z = new State("Z", new Transition(EventType.G, Terminal, 1.0f));
        public static State Y = new State("Y", new Transition(EventType.E, Z, 1.0f));
        public static State X = new State("X", new Transition(EventType.B, Y, 0.2f), new Transition(EventType.D, Z, 0.8f));
        public static State W = new State("W", new Transition(EventType.B, Y, 1.0f));
        public static State Initial = new State("Initial", new Transition(EventType.A, W, 0.6f), new Transition(EventType.C, X, 0.4f));

        private readonly Transition[] _transitions;

        public State(string name, params Transition[] transitions)
        {
            Name = name;
            _transitions = transitions;
        }

        public string Name { get; }

        /// <summary>
        /// Checks if this state is a terminal state.
        /// A terminal state has no outgoing transitions.
        /// </summary>
        public bool IsTerminal => Name == Terminal.Name && _transitions.Length == 0;

        /// <summary>
        /// Gets the state after transitioning from this state based on the given event.
        /// </summary>
        /// <param name="evt">The event that defined the transition.</param>
        /// <returns>The new state, or [[InvalidTransition]].</returns>
        public State Transition(EventType evt)
        {
            foreach (var t in _transitions)
                if (t.EventType == evt)
                    return t.TargetState;

            // no transition found
            return InvalidTransition;
        }
    }
}
