using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Global/Decisions/KeyDown")]
public class DKeyDown : Decision
{
    public KeyCode InputAction;

    public override bool DecideTransition(StateManager controller)
    {
        return Input.GetKeyDown(InputAction);
    }
}
