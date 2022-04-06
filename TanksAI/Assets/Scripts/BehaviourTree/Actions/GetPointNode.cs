

using UnityEngine;

namespace BehaviourTree
{
    public class GetPointNode : ActionNode
    {
        public string indexString;
        public Vector3ListSO map;

        protected override void OnStart()
        {
            if (!blackboard.HasValue(indexString))
            {
                blackboard.SetValue(indexString, 0);
            }
            
            var current = blackboard.GetValue<int>(indexString);
            
            var waypoint = map.Value[current];
            blackboard.SetValue("waypoint", waypoint);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}
