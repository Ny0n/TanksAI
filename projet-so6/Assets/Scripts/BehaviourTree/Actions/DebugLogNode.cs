using UnityEngine;

namespace BehaviourTree
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        protected override void OnStart()
        {
            TankManager tankManager = blackboard.GetValue<TankManager>("tankManager");

            // Debug.Log($"DebugLogNode: Player Number {tankManager.playerNumber}");
            // Debug.Log($"DebugLogNode: Player Number {tankManager.team.Name}");
            // GameObject tank = tankManager.tankInstance;
            // RaycastHit hit;
            // if (Physics.Raycast(tank.transform.position, tank.transform.TransformDirection(Vector3.forward), out hit, 500f))
            // {
            //     Debug.Log(hit.collider.gameObject.name);
            // }
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
