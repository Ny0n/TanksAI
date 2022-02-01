using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "StateMachine/Tank/Decisions/AI/Shoot")]
public class AAIShoot : Decision
{
    public override bool DecideTransition(StateManager controller)
    {
        TankStateManager castController = (TankStateManager) controller;

        return castController.AIShot;
    }
}
