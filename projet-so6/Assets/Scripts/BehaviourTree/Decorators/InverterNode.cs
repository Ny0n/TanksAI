namespace BehaviourTree
{
    public class InverterNode : DecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            State state = child.state switch
            {
                State.Success => State.Failure,
                State.Running => State.Running,
                State.Failure => State.Success,
                _ => State.Failure
            };

            return state;
        }
    }
}
