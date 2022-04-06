using UnityEngine;

namespace BehaviourTree
{
    public class RepeatNode : DecoratorNode
    {
        public int numberRepeat = 0;
        public bool infinite = true;
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (infinite)
            {
                child.Update();
                return State.Running;
            }
    
            if (!infinite)
            {
                if (numberRepeat > 0)
                {
                    child.Update();
                    if (child.state == State.Success)
                    {
                        numberRepeat--;
                    }
                    return State.Running;
                }
                return State.Success;
            }
            return State.Failure;
        }
    }
}
