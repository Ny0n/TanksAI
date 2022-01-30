using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "StateMachine/Tank/Actions/AIMoveTank")]
public class AAIMoveTank : StateAction
{
    public override void ExecuteAction(StateManager controller)
    {
        TankStateManager castController = (TankStateManager) controller;
        
        if(castController.TankPathSystem.MyPath.Count == 0)
            return;
        
        Transform currentTransform = castController.gameObject.transform;
        
        Vector3 nextDestination = castController.TankPathSystem.MyPath[0];
        float angle = Vector3.SignedAngle(currentTransform.forward, nextDestination - currentTransform.position, Vector3.one);
        castController.TankMovement.TurnInputValue = castController.TurnRateCurve.Evaluate(angle);
        castController.TankMovement.MovementInputValue = castController.MoveRateCurve.Evaluate(angle);
            
        if (Vector3.SqrMagnitude(nextDestination - currentTransform.position) < 0.1)
        {
            castController.TankPathSystem.MyPath.RemoveAt(0);
        }
            
        Debug.DrawRay(currentTransform.position, nextDestination - currentTransform.position, Color.blue);
    }

    public override void OnStateActionEnter(StateManager controller)
    {
    }

    public override void OnStateActionExit(StateManager controller)
    {

    }
}
