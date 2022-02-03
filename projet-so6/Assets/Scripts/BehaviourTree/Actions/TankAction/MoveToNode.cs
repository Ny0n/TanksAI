using System.Linq;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveToNode : ActionNode
    {
        private TankPathSystem _tankPathSystem;
        private TankMovement _tankMovement;
        private TankManager _tankManager;

        private bool jumpFrame;

        public string positionName;
        
        [Range(0.1f, 2f)]
        [SerializeField] private float goalPrecision;
        
        protected override void OnStart()
        {
            Debug.Log(positionName);
            _tankManager = blackboard.GetValue<TankManager>("tankManager");
            
            _tankMovement = _tankManager.tankInstance.GetComponent<TankMovement>();
            _tankPathSystem = _tankManager.tankInstance.GetComponent<TankPathSystem>();
            jumpFrame = true;
            
            Vector3 targetPos = blackboard.GetValue<Vector3>(positionName);
            
            Transform currentTransform = _tankManager.tankInstance.transform;
            if (Vector3.SqrMagnitude(targetPos - currentTransform.position) < goalPrecision)
                return;
            
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

            if (Vector3.SqrMagnitude(_tankPathSystem.MyPath.Last() - currentTransform.position) < goalPrecision)
            {
                return State.Success;
            }
            
            _tankPathSystem.Agent.nextPosition = currentTransform.position;

            Debug.Log("MOVEEEEEEEEEE");
            
            Vector3 nextDestination = _tankPathSystem.MyPath[0];
            float angle = Vector3.SignedAngle(currentTransform.forward, nextDestination - currentTransform.position, Vector3.one);
            
            _tankMovement.TurnInputValue = _tankPathSystem.TurnRateCurve.Evaluate(angle);
            _tankMovement.MovementInputValue = _tankPathSystem.MoveRateCurve.Evaluate(angle);
            
            if (Vector3.SqrMagnitude(nextDestination - currentTransform.position) < goalPrecision)
            {
                _tankPathSystem.MyPath.RemoveAt(0);
            }

            return State.Running;
        }
    }
}
