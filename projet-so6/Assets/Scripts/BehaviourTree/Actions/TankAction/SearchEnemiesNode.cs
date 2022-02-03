using UnityEngine;

namespace BehaviourTree
{
    public class SearchEnemiesNode : ActionNode
    {
        public string FoundEnemyKey;
        public string EnemyPosKey;
        
        [Range(10, 500)]
        public float radius;

        private TankManager _tankManager;
        private TankData _myData;
    
        protected override void OnStart()
        {
            _tankManager = blackboard.GetValue<TankManager>("tankManager");
            _myData = _tankManager.tankInstance.GetComponent<TankData>();
            
            blackboard.SetValue(FoundEnemyKey, false);
            blackboard.SetValue(EnemyPosKey, new Vector3());
        }

        protected override void OnStop()
        {
        
        }

        protected override State OnUpdate()
        {
            foreach (var team in blackboard.TeamsList.Value)
            {
                if (team != _myData.Team)
                {
                    foreach (var tm in team.Tanks)
                    {
                        Transform currentPos = _tankManager.tankInstance.transform;
                        Transform enemyPos = tm.tankInstance.transform;

                        RaycastHit hit;
                        if (Physics.Raycast(currentPos.position, currentPos.position - enemyPos.position, out hit,
                                radius))
                        {
                            if (hit.collider.CompareTag("Tank"))
                            {
                                blackboard.SetValue(FoundEnemyKey, true);
                                blackboard.SetValue(EnemyPosKey, enemyPos.position);
                                
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
