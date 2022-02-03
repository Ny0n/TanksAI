using UnityEngine;

namespace BehaviourTree
{
    public class UpdateTargetPos : ActionNode
    {
        public LayerMask tankMask;
        private TankManager _tankManager;
        private TankData myData;

        [Range(10, 500)]
        public float radius;
    
        protected override void OnStart()
        {
            _tankManager = blackboard.GetValue<TankManager>("tankManager");
            
            myData = _tankManager.tankInstance.GetComponent<TankData>();
            
            
            blackboard.SetValue("targetPos", new Vector3());
        }

        protected override void OnStop()
        {
        
        }

        protected override State OnUpdate()
        {
            
            RaycastHit[] hits =
                Physics.SphereCastAll(_tankManager.tankInstance.transform.position, radius,
                    _tankManager.tankInstance.transform.forward);            

            foreach (var raycastHit in hits)
            {
                if (raycastHit.transform.gameObject.CompareTag("Tank"))
                {
                    GameObject targetGo = raycastHit.transform.gameObject;
                    
                    if(targetGo == _tankManager.tankInstance.gameObject)
                        continue;
                    
                    TankData targetData = targetGo.GetComponent<TankData>();

                    if (myData.Team.Name != targetData.Team.Name)
                    {
                        RaycastHit hitRay = new RaycastHit();
                        if (Physics.Raycast(_tankManager.tankInstance.transform.position,
                                targetGo.transform.position - _tankManager.tankInstance.transform.position,
                                out hitRay, radius))
                        {
                            if (hitRay.transform.gameObject == targetGo)
                            {
                                blackboard.SetValue("targetPos", hitRay.transform.position);
                                return State.Success;
                            }
                        }
                    }
                }
            }
        
            return State.Failure;
        }
    }
}
