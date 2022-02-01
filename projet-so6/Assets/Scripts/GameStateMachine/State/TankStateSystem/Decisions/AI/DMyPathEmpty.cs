using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Decisions/AI/MyPathEmpty")]
public class DMyPathEmpty : Decision
{
    public override bool DecideTransition(StateManager controller)
    {
        TankStateManager castController = (TankStateManager)controller;

        return castController.TankPathSystem.MyPath.Count == 0;
    }
}
