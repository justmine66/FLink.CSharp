using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Streaming.Api.DataStreams;
using FLink.Streaming.Api.Environment;

namespace UserActionsEvaluation
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = StreamExecutionEnvironment.GetExecutionEnvironment();

            DataStream<UserAction> actions = null;
            DataStream<ActionPattern> patterns = null;

            var actionsByUser = actions.KeyBy(action => action.UserId);

            var bcStateDescriptor = new MapStateDescriptor<string, ActionPattern>("patterns", Types.String, Types.Poco<ActionPattern>());

            var bcedPatterns = patterns.Broadcast(bcStateDescriptor);

            var matches = actionsByUser
                .Connect(bcedPatterns)
                .Process(new PatternEvaluator());

            matches.Print();

            env.Execute("User Actions Evaluation");
        }
    }
}
