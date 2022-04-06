using System.Linq;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveToNode : ActionNode
    {
        private TankPathSystem _tankPathSystem;
        private TankMovement _tankMovement;
        private TankManager _tankManager;

        private bool doNode;

        public string positionName;
        
        [Range(0.1f, 10f)]
        [SerializeField] private float goalPrecision;
        
        protected override void OnStart()
        {
            _tankManager = blackboard.GetValue<TankManager>("tankManager");
            
            _tankMovement = _tankManager.tankInstance.GetComponent<TankMovement>();
            _tankPathSystem = _tankManager.tankInstance.GetComponent<TankPathSystem>();
            doNode = true;
            
            Vector3 targetPos = blackboard.GetValue<Vector3>(positionName);
            
            Transform currentTransform = _tankManager.tankInstance.transform;
            if (Vector3.SqrMagnitude(targetPos - currentTransform.position) < goalPrecision)
            {
                doNode = false;
                return;
            }
            
            _tankPathSystem.SearchTargetPath(targetPos);
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            if (!_tankManager.tankInstance.activeSelf)
            {
                return State.Failure;
            }
            
            if (!doNode)
                return State.Success;
            
            Transform currentTransform = _tankManager.tankInstance.transform;
            
            if (_tankPathSystem.MyPath.Count > 0 && Vector3.SqrMagnitude(_tankPathSystem.MyPath.Last() - currentTransform.position) < goalPrecision)
            {
                _tankMovement.TurnInputValue = 0;
                _tankMovement.MovementInputValue = 0;
                _tankPathSystem.MyPath.Clear();
                return State.Success;
            }
            
            if (_tankPathSystem.MyPath.Count == 0)
            {
                return State.Running;
            }

            
            _tankPathSystem.Agent.nextPosition = currentTransform.position;
            
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
