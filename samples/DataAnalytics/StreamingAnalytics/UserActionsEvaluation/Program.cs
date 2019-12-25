using FLink.Core.Api.Common.States;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Streaming.Api.DataStreams;
using FLink.Streaming.Api.Environments;

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
