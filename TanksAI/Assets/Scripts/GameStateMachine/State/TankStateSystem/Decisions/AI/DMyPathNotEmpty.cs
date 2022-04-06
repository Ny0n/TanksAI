using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Decisions/AI/MyPathNotEmpty")]
public class DMyPathNotEmpty : Decision
{
    public override bool DecideTransition(StateManager controller)
    {
        TankStateManager castController = (TankStateManager)controller;

        return castController.TankPathSystem.MyPath.Count > 0;
    }
}
