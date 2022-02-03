using System.Collections.Generic;

namespace BehaviourTree
{
    public class ConditionNode : DecoratorNode
    {
        public List<BoolVariableSO> boolList;
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            foreach (var b in boolList)
            {
                if (!b.Value)
                {
                    return State.Failure;
                }
            }

            child.Update();
            return State.Success;
        }
    }
}
