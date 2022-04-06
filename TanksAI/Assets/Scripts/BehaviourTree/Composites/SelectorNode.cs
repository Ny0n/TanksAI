using System;
using UnityEngine;

namespace BehaviourTree
{
    public class SelectorNode : CompositeNode
    {
        private int current = 0;
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var child = children[current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    current++;
                    break;
                case State.Success:
                    return State.Success;
            }

            return current == children.Count ? State.Failure : State.Running;
        }
    }
}
