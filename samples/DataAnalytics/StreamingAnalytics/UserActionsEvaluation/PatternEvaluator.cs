using FLink.Core.Api.Common.States;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Configurations;
using FLink.Core.Util;
using FLink.Streaming.Api.Functions.Co;

namespace UserActionsEvaluation
{
    public class PatternEvaluator : KeyedBroadcastProcessFunction<long, UserAction, ActionPattern, (long, ActionPattern)>
    {
        // handle for keyed state (per user)
        private IValueState<string> _prevActionState;
        // broadcast state descriptor
        private MapStateDescriptor<string, ActionPattern> _patternDesc;

        public override void Open(Configuration parameters)
        {
            _prevActionState = RuntimeContext.GetState(new ValueStateDescriptor<string>("lastAction", Types.String));
            _patternDesc = new MapStateDescriptor<string, ActionPattern>("patterns", Types.String, Types.Poco<ActionPattern>());
        }

        public override void ProcessElement(UserAction userAction, ReadOnlyContext context, ICollector<(long, ActionPattern)> output)
        {
            // get current pattern from broadcast state
            var pattern = context
                .GetBroadcastState(_patternDesc)
                .Get(null); // access MapState with null as VOID default value

            // get previous action of current user from keyed state
            var prevAction = _prevActionState.Value;
            if (pattern != null && prevAction != null)
            {
                // user had an action before, check if pattern matches
                if (pattern.FirstAction.Equals(prevAction) &&
                    pattern.SecondAction.Equals(userAction.Action))
                {
                    // MATCH
                    output.Collect((context.CurrentKey, pattern));
                }
            }

            // update keyed state and remember action for next pattern evaluation
            _prevActionState.Value = userAction.Action;
        }

        public override void ProcessBroadcastElement(ActionPattern pattern, Context context, ICollector<(long, ActionPattern)> output)
        {
            // store the new pattern by updating the broadcast state
            var bcState = context.GetBroadcastState(_patternDesc);
            // storing in MapState with null as VOID default value
            bcState.Put(null, pattern);
        }
    }
}
