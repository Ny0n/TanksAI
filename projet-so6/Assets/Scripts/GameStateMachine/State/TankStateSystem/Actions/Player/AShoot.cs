using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "StateMachine/Tank/Actions/Shoot")]
public class AShoot : StateAction
{
    public override void ExecuteAction(StateManager controller)
    {
        TankStateManager castController = (TankStateManager) controller;
        
        castController.TankShooting.TryToFire();
    }

    public override void OnStateActionEnter(StateManager controller)
    {
        
    }

    public override void OnStateActionExit(StateManager controller)
    {
        
    }
}
