using UnityEngine;

namespace BehaviourTree
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        protected override void OnStart()
        {
            Debug.Log($"DebugLogNode: Team {teamsList.Value[0].Name}");
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
