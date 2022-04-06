using System.Collections;
using UnityEngine;

namespace BehaviourTree
{
    public class TimeoutNode : DecoratorNode
    {
        public CooldownSO timeoutSO;
        private bool _hasStarted = false;
        protected override void OnStart()
        {
            if (!_hasStarted)
            {
                timeoutSO.StartCooldown();
                _hasStarted = true;
            }
        }

        protected override void OnStop()
        {
        }

        
        protected override State OnUpdate()
        {
            if (timeoutSO.IsCooldownDone())
            {
                return State.Failure;
            }
            
            child.Update();
            
            return State.Success;
        }
    }
}
