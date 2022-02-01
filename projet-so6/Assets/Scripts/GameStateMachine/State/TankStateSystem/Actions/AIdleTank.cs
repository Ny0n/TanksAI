using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Actions/IdleTank")]
public class AIdleTank : StateAction
{
    public override void ExecuteAction(StateManager controller)
    {
        
    }

    public override void OnStateActionEnter(StateManager controller)
    {
        TankStateManager castController = (TankStateManager) controller;

        castController.TankMovement.TurnInputValue = 0;
        castController.TankMovement.MovementInputValue = 0;
    }

    public override void OnStateActionExit(StateManager controller)
    {
        
    }
}
