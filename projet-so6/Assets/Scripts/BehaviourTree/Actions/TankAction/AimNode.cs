using UnityEngine;

namespace BehaviourTree
{
    public class AimNode : ActionNode
    {
        private TankStateManager _tankStateManager;
        private TankMovement _tankMovement;
        private TankManager _tankManager;
        private Vector3 _targetPos;

        protected override void OnStart()
        {
            _tankManager = blackboard.GetValue<TankManager>("tankManager");

            _tankMovement = _tankManager.tankInstance.GetComponent<TankMovement>();
            _tankStateManager = _tankManager.tankInstance.GetComponent<TankStateManager>();

            _targetPos = blackboard.GetValue<Vector3>("targetPos");
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            Transform currentTransform = _tankManager.tankInstance.transform;
            
            float angle = Vector3.SignedAngle(currentTransform.forward, _targetPos - currentTransform.position, Vector3.one);
            _tankMovement.TurnInputValue = _tankStateManager.TurnRateCurve.Evaluate(angle);
            
            return Mathf.Abs(angle) <= 0.01 ? State.Success : State.Running;
        }
    }
}
