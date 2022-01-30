using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Actions/PlayerMoveTank")]
public class APlayerMoveTank : StateAction
{
    public override void ExecuteAction(StateManager controller)
    {
        TankStateManager castController = (TankStateManager) controller;

        castController.TankMovement.TurnInputValue = Input.GetAxis(castController.TankMovement.m_TurnAxisName);
        castController.TankMovement.MovementInputValue = Input.GetAxis(castController.TankMovement.m_MovementAxisName);
    }

    public override void OnStateActionEnter(StateManager controller)
    {
        
    }

    public override void OnStateActionExit(StateManager controller)
    {
        
    }
}
