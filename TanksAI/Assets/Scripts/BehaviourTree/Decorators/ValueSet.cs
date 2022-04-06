namespace BehaviourTree
{
    public class ValueSet : DecoratorNode
    {
        public string keyTestNull;

        public bool testIfNull;
        protected override void OnStart()
        {
        
        }

        protected override void OnStop()
        {
        
        }

        protected override State OnUpdate()
        {
            bool result = blackboard.GetValue<object>(keyTestNull) == null;

            result = testIfNull ? result : !result;
        
            if (result)
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}
