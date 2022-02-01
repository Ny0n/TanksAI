using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Actions/AI/AvoidObstacle")]
public class AAIAvoidObstacle : StateAction
{
    public override void ExecuteAction(StateManager controller)
    {
        TankStateManager castController = (TankStateManager) controller;

        float angle = castController.AITankAvoidance.AngleCost;
        
        castController.TankMovement.TurnInputValue = -castController.TurnRateCurve.Evaluate(angle);
        castController.TankMovement.MovementInputValue = 1;
         
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
