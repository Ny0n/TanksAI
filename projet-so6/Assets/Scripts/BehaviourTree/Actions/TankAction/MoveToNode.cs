using UnityEngine;

namespace BehaviourTree
{
    public class MoveToNode : ActionNode
    {
        private TankPathSystem _tankPathSystem;
        private TankStateManager _tankStateManager;
        private TankMovement _tankMovement;
        private TankManager _tankManager;

        private bool jumpFrame;

        public string positionName;
        
        [Range(0.1f, 2f)]
        [SerializeField] private float goalPrecision;
        
        protected override void OnStart()
        {
            _tankManager = blackboard.GetValue<TankManager>("tankManager");
            
            _tankMovement = _tankManager.tankInstance.GetComponent<TankMovement>();
            _tankPathSystem = _tankManager.tankInstance.GetComponent<TankPathSystem>();
            _tankStateManager = _tankManager.tankInstance.GetComponent<TankStateManager>();
            jumpFrame = true;
            
            Vector3 targetPos = blackboard.GetValue<Vector3>(positionName);
            
            _tankPathSystem.SearchTargetPath(targetPos);
        }

        protected override void OnStop()
        {
            _tankMovement.TurnInputValue = 0;
            _tankMovement.MovementInputValue = 0;
        }

        protected override State OnUpdate()
        {
            if (jumpFrame)
            {
                jumpFrame = false;
                return State.Running;
            }
            
            if (_tankPathSystem.MyPath.Count <= 0)
                return State.Success;
            
            Transform currentTransform = _tankManager.tankInstance.transform;

            _tankPathSystem.Agent.nextPosition = currentTransform.position;
        
            Vector3 nextDestination = _tankPathSystem.MyPath[0];
            float angle = Vector3.SignedAngle(currentTransform.forward, nextDestination - currentTransform.position, Vector3.one);
            _tankMovement.TurnInputValue = _tankStateManager.TurnRateCurve.Evaluate(angle);
            _tankMovement.MovementInputValue = _tankStateManager.MoveRateCurve.Evaluate(angle);
            
            if (Vector3.SqrMagnitude(nextDestination - currentTransform.position) < goalPrecision)
            {
                _tankPathSystem.MyPath.RemoveAt(0);
            }

            return State.Running;
        }
    }
}
