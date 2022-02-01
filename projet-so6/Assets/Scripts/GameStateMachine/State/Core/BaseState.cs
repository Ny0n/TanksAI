using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[Serializable]
[CreateAssetMenu (menuName = "StateMachine/BaseState")]
public class BaseState : ScriptableObject
{
    [SerializeField] private StateAction[] stateActions;
    [SerializeField] private StateTransition stateTransitions;
    
    public void Tick(StateManager controller, int stateIndex)
    {
        MakeTransition(controller, stateIndex);
        ExecuteState(controller);
    }

    public void OnStateEnter(StateManager controller)
    {
        foreach (var action in stateActions)
        {
            action.OnStateActionEnter(controller);
        }
    }
    
    public void OnStateExit(StateManager controller)
    {
        foreach (var action in stateActions)
        {
            action.OnStateActionExit(controller);
        }
    }

    private void MakeTransition(StateManager controller, int stateIndex)
    {
        foreach (var tuple in stateTransitions.m_Transitions)
        {
            if(tuple.Decision.DecideTransition(controller))
            {
                controller.ChangeState(tuple.nextState, stateIndex);
            }
        }
    }

    private void ExecuteState(StateManager controller)
    {
        foreach (var action in stateActions)
        {
            action.ExecuteAction(controller);
        }
    }

}
