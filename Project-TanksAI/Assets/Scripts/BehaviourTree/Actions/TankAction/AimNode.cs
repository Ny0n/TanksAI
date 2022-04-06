using UnityEngine;

namespace BehaviourTree
{
    public class AimNode : ActionNode
    {
        private TankPathSystem _tankPathSystem;
        private TankMovement _tankMovement;
        private TankManager _tankManager;
        private Vector3 _targetPos;

        public string TargetPosKey;

        protected override void OnStart()
        {
            _tankManager = blackboard.GetValue<TankManager>("tankManager");

            _tankMovement = _tankManager.tankInstance.GetComponent<TankMovement>();
            _tankPathSystem = _tankManager.tankInstance.GetComponent<TankPathSystem>();

            _targetPos = blackboard.GetValue<Vector3>(TargetPosKey);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            Transform currentTransform = _tankManager.tankInstance.transform;
            
            float angle = Vector3.SignedAngle(currentTransform.forward, _targetPos - currentTransform.position, Vector3.one);
            _tankMovement.TurnInputValue = _tankPathSystem.TurnRateCurve.Evaluate(angle);
            
            return Mathf.Abs(angle) <= 0.1f ? State.Success : State.Running;
        }
    }
}
