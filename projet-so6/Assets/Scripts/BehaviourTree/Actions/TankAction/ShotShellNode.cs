namespace BehaviourTree
{
    public class ShotShellNode : ActionNode
    {
        private TankShooting _tankShooting;
    
        protected override void OnStart()
        {
            var tankManager = (TankManager) blackboard.Values["tankManager"];
            _tankShooting = tankManager.tankInstance.GetComponent<TankShooting>();
        }

        protected override void OnStop()
        {
        
        }

        protected override State OnUpdate()
        {
            _tankShooting.TryToFire();
            return State.Success;
        }
    }
}
