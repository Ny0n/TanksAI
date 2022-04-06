using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "StateMachine/Global/Decisions/True")]
public class DTrue : Decision
{
    public override bool DecideTransition(StateManager controller)
    {
        return true;
    }
}
