using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Decisions/AxisValue")]
public class DAxisValue : Decision
{
    public bool reverseResult;
    
    public override bool DecideTransition(StateManager controller)
    {
        TankStateManager castController = (TankStateManager)controller;
        if (castController.TankMovement.m_MovementAxisName == "")
            return false;
        
        bool result = Input.GetAxis(castController.TankMovement.m_MovementAxisName) != 0 
                      || Input.GetAxis(castController.TankMovement.m_TurnAxisName) != 0;

        result = reverseResult ? !result : result;

        return result;
    }
}
