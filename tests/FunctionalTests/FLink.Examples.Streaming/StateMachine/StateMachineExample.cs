using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.State;
using FLink.Core.Configurations;
using FLink.Core.Util;
using FLink.Examples.Streaming.StateMachine.DFA;
using FLink.Examples.Streaming.StateMachine.Events;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions.Source;

namespace FLink.Examples.Streaming.StateMachine
{
    /// <summary>
    /// Main class of the state machine example.
    /// This class implements the streaming application that receives the stream of events and evaluates a state machine (per originating address) to validate that the events follow the state machine's rules.
    /// </summary>
    public class StateMachineExample
    {
        public static void Run()
        {
            // create the environment to create streams and configure execution
            var env = StreamExecutionEnvironment.GetExecutionEnvironment();
            env.EnableCheckpointing(2000);

            ISourceFunction<Event> source = default;
            var events = env.AddSource(source);

            var alerts = events
                // partition on the address to make sure equal addresses end up in the same state machine flatMap function
                .KeyBy(nameof(Event.SourceAddress))
                // the function that evaluates the state machine over the sequence of events
                .FlatMap(new StateMachineMapper());

            alerts.Print();

            env.Execute("State machine job");
        }

        /// <summary>
        /// The function that maintains the per-IP-address state machines and verifies that the events are consistent with the current state of the state machine. If the event is not consistent with the current state, the function produces an alert.
        /// </summary>
        public class StateMachineMapper : RichFlatMapFunction<Event, Alert>
        {
            // The state for the current key.
            private IValueState<State> _currentState;

            public override void Open(Configuration parameters)
            {
                // get access to the state object
                _currentState = RuntimeContext.GetState(new ValueStateDescriptor<State>("state"));
            }

            public override void FlatMap(Event @event, ICollector<Alert> output)
            {   
                // get the current state for the key (source address)
                // if no state exists, yet, the state must be the state machine's initial state
                var state = _currentState.Value ?? State.Initial;

                // ask the state machine what state we should go to based on the given event type
                var nextState = state.Transition(@event.Type);

                if (nextState == State.InvalidTransition)
                {
                    // the current event resulted in an invalid transition
                    // raise an alert!
                    output.Collect(new Alert(@event.SourceAddress, state, @event.Type));
                }
                else if (nextState.IsTerminal)
                {
                    // we reached a terminal state, clean up the current state
                    _currentState.Clear();
                }
                else
                {
                    // remember the new state
                    _currentState.Value = nextState;
                }

            }
        }
    }
}
