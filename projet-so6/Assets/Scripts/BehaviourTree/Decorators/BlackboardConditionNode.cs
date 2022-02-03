using System.Collections.Generic;

namespace BehaviourTree
{
    public class BlackboardConditionNode : DecoratorNode
    {
        public string key;
        public bool invert;
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (!invert)
            {
                if (!blackboard.GetValue<bool>(key))
                    return State.Failure;

                return child.Update();
            }
            else
            {
                if (!blackboard.GetValue<bool>(key))
                {
                    return child.Update();
                }

                return State.Failure;
            }
            
        }
    }
}
