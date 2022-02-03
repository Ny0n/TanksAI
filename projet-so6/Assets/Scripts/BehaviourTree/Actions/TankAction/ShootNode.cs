namespace BehaviourTree
{
    public class ShootNode : ActionNode
    {
        private TankShooting _tankShooting;
    
        protected override void OnStart()
        {
            var tankManager = blackboard.GetValue<TankManager>("tankManager");
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
