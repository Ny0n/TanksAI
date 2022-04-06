using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "StateMachine/Tank/Actions/AI/MoveTank")]
public class AAIMoveTank : StateAction
{
    [Range(0.1f, 2f)]
    [SerializeField] private float goalPrecision;
    public override void ExecuteAction(StateManager controller)
    {
        TankStateManager castController = (TankStateManager) controller;
        
        if(castController.TankPathSystem.MyPath.Count == 0)
            return;
        
        Transform currentTransform = castController.gameObject.transform;

        castController.TankPathSystem.Agent.nextPosition = currentTransform.position;
        
        Vector3 nextDestination = castController.TankPathSystem.MyPath[0];
        float angle = Vector3.SignedAngle(currentTransform.forward, nextDestination - currentTransform.position, Vector3.one);
        castController.TankMovement.TurnInputValue = castController.TankPathSystem.TurnRateCurve.Evaluate(angle);
        castController.TankMovement.MovementInputValue = castController.TankPathSystem.MoveRateCurve.Evaluate(angle);
            
        if (Vector3.SqrMagnitude(nextDestination - currentTransform.position) < goalPrecision)
        {
            castController.TankPathSystem.MyPath.RemoveAt(0);
        }
    }

    public override void OnStateActionEnter(StateManager controller)
    {
    }

    public override void OnStateActionExit(StateManager controller)
    {

    }
}
