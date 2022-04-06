using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu (menuName = "StateMachine/StateTransition")]
public class StateTransition : ScriptableObject
{
    public DecisionTuple[] m_Transitions;
}

[Serializable]
public struct DecisionTuple
{
    public Decision Decision;
    public BaseState nextState;
}
