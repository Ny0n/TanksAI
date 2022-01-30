using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class StateAction : ScriptableObject
{
    public abstract void ExecuteAction(StateManager controller);
    public abstract void OnStateActionEnter(StateManager controller);
    public abstract void OnStateActionExit(StateManager controller);
}
