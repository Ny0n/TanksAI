using UnityEngine;

namespace BehaviourTree
{
    public class CheckTankNode : DecoratorNode
    {
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var tankManager = blackboard.GetValue<TankManager>("tankManager");
            Debug.Log(tankManager.tankInstance.activeSelf);
            return tankManager.tankInstance.activeSelf ? child.Update() : State.Failure;
        }
    }
}
