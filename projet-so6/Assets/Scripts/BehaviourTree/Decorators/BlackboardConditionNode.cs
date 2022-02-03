using System.Collections.Generic;

namespace BehaviourTree
{
    public class BlackboardConditionNode : DecoratorNode
    {
        public string key;
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (!blackboard.GetValue<bool>(key))
                return State.Failure;

            child.Update();
            return State.Success;
        }
    }
}
