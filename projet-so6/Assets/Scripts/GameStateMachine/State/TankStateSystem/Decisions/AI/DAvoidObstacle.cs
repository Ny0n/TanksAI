using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Decisions/AI/AvoidObstacle")]
public class DAvoidObstacle : Decision
{
    public bool ReverseResult;
    
    public override bool DecideTransition(StateManager controller)
    {
        TankStateManager castController = (TankStateManager) controller;
        
        bool result = castController.AITankAvoidance.AvoidObstacle;

        return ReverseResult ? !result : result;
    }
}
