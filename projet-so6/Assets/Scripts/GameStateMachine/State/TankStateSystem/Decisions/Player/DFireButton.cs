using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Decisions/Player/FireButton")]
public class DFireButton : Decision
{
    public override bool DecideTransition(StateManager controller)
    {
        TankStateManager castController = (TankStateManager)controller;
        if (castController.TankShooting.FireButton == "")
            return false;

        return Input.GetButtonDown(castController.TankShooting.FireButton);
    }
}
